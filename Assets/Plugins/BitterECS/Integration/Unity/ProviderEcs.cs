using System;
using System.Collections.Generic;
using System.Reflection;
using BitterECS.Core;
using UnityEngine;

namespace BitterECS.Integration
{
    public interface ITypedComponentProvider
    {
        void Sync(EcsEntity entity);
    }

    public abstract class ProviderEcs : MonoBehaviour, ITypedComponentProvider, ILinkableProvider
    {
        public abstract bool IsPresenter { get; }
        public abstract EcsEntity Entity { get; }

        internal abstract EcsEntity GetEntitySilently();

        public virtual EcsProperty Properties => GetEntitySilently()?.Properties;

        public abstract void Sync(EcsEntity entity);
        public void Dispose() => DisposeInternal();

        protected abstract void InitInternal(EcsProperty property);
        protected abstract void DisposeInternal();

        void IInitialize<EcsProperty>.Init(EcsProperty property) => InitInternal(property);
    }

    [DisallowMultipleComponent]
    public class ProviderEcs<T> : ProviderEcs where T : new()
    {
        private delegate void SyncDelegate(EcsEntity entity, in T value);

        private static readonly bool s_isPresenterType = typeof(EcsPresenter).IsAssignableFrom(typeof(T));
        private static readonly bool s_isValueType = typeof(T).IsValueType;
        private static readonly SyncDelegate s_syncAction;
        private static readonly List<ITypedComponentProvider> s_sharedComponentCache = new(16);

        static ProviderEcs()
        {
            if (!s_isValueType || s_isPresenterType) return;

            try
            {
                var methodInfo = typeof(EcsEntity).GetMethod(nameof(EcsEntity.AddOrReplace), BindingFlags.Instance | BindingFlags.Public);
                if (methodInfo != null)
                {
                    var genericMethod = methodInfo.MakeGenericMethod(typeof(T));
                    s_syncAction = (SyncDelegate)Delegate.CreateDelegate(typeof(SyncDelegate), genericMethod);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"[ProviderEcs] Delegate creation failed for {typeof(T)}: {e}");
            }
        }

        [SerializeField] protected T _value;

        private bool _isDestroying;
        private EcsProperty _properties;
        private ProviderEcs _cachedRootProvider;

        public override bool IsPresenter => s_isPresenterType;

        public override EcsEntity Entity
        {
            get
            {
                var entity = GetEntitySilently() ?? throw new Exception(
                    $"[ProviderEcs<{typeof(T).Name}>] Entity is not linked on '{name}'. Use NewEntity() for prefabs.");
                return entity;
            }
        }

        public ref T Value => ref SyncInspectorValue(ref Entity.Get<T>());

        internal override EcsEntity GetEntitySilently()
        {
            if (s_isPresenterType)
            {
                if (_properties?.Presenter != null)
                {
                    return _properties.Presenter.Get(_properties.Id);
                }
                return null;
            }
            return GetParentEntitySilently();
        }

        public EcsEntity NewEntity()
        {
            try
            {
                return CreateEntity();
            }
            catch (Exception ex)
            {
                throw new Exception($"[ProviderEcs<{typeof(T).Name}>] Factory error: {ex.Message}");
            }
        }

        protected virtual void Awake()
        {
            if (s_isPresenterType)
            {
                InitializeAsPresenter();
            }
            else
            {
                ProcessComponents();
                if (_cachedRootProvider == null)
                {
                    throw new Exception($"[ProviderEcs<{typeof(T).Name}>] Missing Root Provider on '{name}'.");
                }
            }
        }

        //#if UNITY_EDITOR
        //        private void Update()
        //        {
        //            var entity = GetEntitySilently();
        //            if (entity != null && entity.TryGet<T>(out var component))
        //            {
        //                SyncInspectorValue(ref component);
        //            }
        //        }
        //#endif

        private void OnDestroy() => Dispose();

        public override void Sync(EcsEntity entity)
        {
            if (s_isPresenterType || !s_isValueType || entity == null) return;
            s_syncAction?.Invoke(entity, _value);
        }

        protected override void InitInternal(EcsProperty property)
        {
            if (s_isPresenterType) _properties = property;
        }

        protected override void DisposeInternal()
        {
            if (_isDestroying) return;
            _isDestroying = true;

            var isSceneObject = gameObject != null && gameObject.scene.IsValid();

            if (isSceneObject)
            {
                if (s_isPresenterType)
                {
                    HandlePresenterDispose();
                }
                else
                {
                    GetEntitySilently()?.Dispose();
                }

                Destroy(gameObject);
            }
            else
            {
                _properties = null;
                _cachedRootProvider = null;
            }
        }

        private void InitializeAsPresenter()
        {
            if (_isDestroying) return;
            if (_properties?.Presenter != null) return;

            CreateEntity();
        }

        private EcsEntity CreateEntity()
        {
            return Build.For(typeof(T))
                     .Add<EcsEntity>()
                     .WithPost(ProcessComponents)
                     .WithLink(this)
                     .Create();
        }

        private void ProcessComponents(EcsEntity entityToSync = null)
        {
            if (_isDestroying) return;
            var isSyncMode = entityToSync != null;

            GetComponents(s_sharedComponentCache);
            foreach (var provider in s_sharedComponentCache)
            {
                if (ReferenceEquals(provider, this)) continue;
                if (isSyncMode) provider.Sync(entityToSync);
                else if (provider is ProviderEcs { IsPresenter: true } root)
                {
                    _cachedRootProvider = root;
                    break;
                }
            }
            s_sharedComponentCache.Clear();
        }

        private void HandlePresenterDispose()
        {
            if (_properties == null) return;
            var presenter = _properties.Presenter;
            if (presenter != null && presenter.Has(_properties.Id))
            {
                presenter.Remove(presenter.Get(_properties.Id));
            }
            _properties = null;
        }

        private ref T SyncInspectorValue(ref T v)
        {
            _value = v;
            return ref v;
        }

        private EcsEntity GetParentEntitySilently()
        {
            if (_cachedRootProvider == null) ProcessComponents();
            return _cachedRootProvider?.GetEntitySilently();
        }
    }
}
