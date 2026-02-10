using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BitterECS.Core
{
    public abstract class EcsPresenter : IDisposable
    {
        private int[] _aliveIds;
        private int[] _idToIndex;
        private int _aliveCount;
        private int _nextId;

        private readonly Stack<int> _freeEntityIds;
        internal IPoolDestroy[] _poolsById;
        private readonly Dictionary<int, ILinkableProvider> _linkedProviders;
        private int[] _componentCounts;

        public int CountEntity => _aliveCount;

        public EcsPresenter()
        {
            var capacity = EcsConfig.InitialEntitiesCapacity;
            _aliveIds = new int[capacity];
            _idToIndex = new int[capacity];
            Array.Fill(_idToIndex, -1);
            _componentCounts = new int[capacity];

            _freeEntityIds = new Stack<int>(capacity);
            _poolsById = new IPoolDestroy[EcsConfig.InitialPoolsCapacity];
            _linkedProviders = new Dictionary<int, ILinkableProvider>(EcsConfig.InitialLinkedEntitiesCapacity);
            _aliveCount = 0;
            _nextId = 0;

            Registration();
        }

        protected virtual void Registration() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddPoolFactory<T>(Func<EcsPool<T>> factory) where T : new()
        {
            var id = ComponentTypeMap<T>.Id;
            if (id >= _poolsById.Length) Array.Resize(ref _poolsById, Math.Max(id + 1, _poolsById.Length * 2));
            _poolsById[id] = factory();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCheckEvent<T>() where T : new()
        {
            var id = ComponentTypeMap<T>.Id;
            if (id >= _poolsById.Length) Array.Resize(ref _poolsById, Math.Max(id + 1, _poolsById.Length * 2));
            if (_poolsById[id] == null) _poolsById[id] = new EcsEventPool<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<int> GetAliveIds() => _aliveIds.AsSpan(0, _aliveCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsEntity CreateEntity()
        {
            int id;
            if (_freeEntityIds.Count > 0)
            {
                id = _freeEntityIds.Pop();
            }
            else
            {
                id = _nextId++;
                if (id >= _idToIndex.Length) ResizeIdArrays(id * 2);
            }

            _idToIndex[id] = _aliveCount;
            _aliveIds[_aliveCount++] = id;

            EcsWorld.IncreaseVersion();
            return new EcsEntity(this, id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(EcsEntity entity)
        {
            var id = entity.Id;
            if (id < 0 || id >= _idToIndex.Length) return;

            var index = _idToIndex[id];
            if (index == -1) return;

            var len = _poolsById.Length;
            for (var i = 0; i < len; i++)
            {
                _poolsById[i]?.Remove(id);
            }

            if (_linkedProviders.Count > 0 && _linkedProviders.Remove(id, out var provider))
            {
                provider.Dispose();
            }

            _idToIndex[id] = -1;
            _aliveCount--;

            if (_aliveCount > 0 && index != _aliveCount)
            {
                var lastId = _aliveIds[_aliveCount];
                _aliveIds[index] = lastId;
                _idToIndex[lastId] = index;
            }

            _componentCounts[id] = 0;
            _freeEntityIds.Push(id);

            EcsWorld.IncreaseVersion();
        }

        private void ResizeIdArrays(int newSize)
        {
            Array.Resize(ref _aliveIds, newSize);
            Array.Resize(ref _componentCounts, newSize);
            var oldSize = _idToIndex.Length;
            Array.Resize(ref _idToIndex, newSize);
            for (var i = oldSize; i < newSize; i++) _idToIndex[i] = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsPool<T> GetPool<T>() where T : new()
        {
            var id = ComponentTypeMap<T>.Id;
            if (id >= _poolsById.Length) Array.Resize(ref _poolsById, Math.Max(id + 1, _poolsById.Length * 2));

            var pool = _poolsById[id];
            if (pool != null) return (EcsPool<T>)pool;

            var newPool = new EcsPool<T>();
            _poolsById[id] = newPool;
            return newPool;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(int id) => id >= 0 && id < _idToIndex.Length && _idToIndex[id] != -1;

        public ILinkableProvider GetProvider(EcsEntity entity)
            => _linkedProviders.TryGetValue(entity.Id, out var provider) ? provider : null;

        public void Link(EcsEntity entity, ILinkableProvider provider)
        {
            _linkedProviders[entity.Id] = provider;
            provider.Init(new EcsProperty(this, entity.Id));
        }

        public void Unlink(EcsEntity entity)
        {
            if (_linkedProviders.Remove(entity.Id, out var provider))
            {
                provider.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsEntity Get(int id) => Has(id) ? new EcsEntity(this, id) : throw new("Entity not created");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void IncrementCount(int id) => _componentCounts[id]++;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void DecrementCount(int id) => _componentCounts[id]--;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetComponentCount(int id) => _componentCounts[id];

        public void Dispose()
        {
            for (int i = 0; i < _poolsById.Length; i++)
            {
                if (_poolsById[i] is IDisposable d) d.Dispose();
            }
            _poolsById = Array.Empty<IPoolDestroy>();

            if (_linkedProviders.Count > 0)
            {
                foreach (var p in _linkedProviders.Values) p?.Dispose();
                _linkedProviders.Clear();
            }

            _freeEntityIds.Clear();
            _aliveCount = 0;
            _nextId = 0;
            _aliveIds = Array.Empty<int>();
            _idToIndex = Array.Empty<int>();
            _componentCounts = Array.Empty<int>();
        }
    }
}
