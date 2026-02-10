using System;
using System.Runtime.CompilerServices;

namespace BitterECS.Core
{
    public struct EcsFilter
    {
        private readonly EcsPresenter _presenter;
        private IHasPool[] _includePools;
        private IHasPool[] _excludePools;
        private Predicate<EcsEntity>[] _predicates;
        private int _includeCount;
        private int _excludeCount;
        private int _predicateCount;

        private RefWorldVersion _refWorld;
        private int[] _filteredCache;
        private int _filteredLength;

        public EcsFilter(EcsPresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _includePools = new IHasPool[EcsConfig.FilterConditionInclude];
            _excludePools = new IHasPool[EcsConfig.FilterConditionExclude];
            _predicates = new Predicate<EcsEntity>[EcsConfig.FilterPredicate];
            _includeCount = 0;
            _excludeCount = 0;
            _predicateCount = 0;
            _refWorld = new();
            _filteredCache = new int[presenter.CountEntity + 32];
            _filteredLength = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter Include<T>() where T : new()
        {
            AddPool(ref _includePools, ref _includeCount, _presenter.GetPool<T>());
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter Include<T>(Predicate<T> predicate) where T : new()
        {
            Include<T>();
            AddPredicate(e => predicate(e.Get<T>()));
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter Exclude<T>() where T : new()
        {
            AddPool(ref _excludePools, ref _excludeCount, _presenter.GetPool<T>());
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter Where(Predicate<EcsEntity> predicate)
        {
            AddPredicate(predicate);
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter WhereProvider<T>(Predicate<T> predicate) where T : class, ILinkableProvider
        {
            AddPredicate(e => e.TryGetProvider<T>(out var p) && predicate(p));
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter WhereProvider<T>() where T : class, ILinkableProvider
        {
            AddPredicate(e => e.HasProvider<T>());
            return this;
        }

        private void AddPool(ref IHasPool[] pools, ref int count, IHasPool pool)
        {
            if (count >= pools.Length) Array.Resize(ref pools, pools.Length * 2);
            pools[count++] = pool;
        }

        private void AddPredicate(Predicate<EcsEntity> predicate)
        {
            if (_predicateCount >= _predicates.Length) Array.Resize(ref _predicates, _predicates.Length * 2);
            _predicates[_predicateCount++] = predicate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RebuildCache()
        {
            var aliveIds = _presenter.GetAliveIds();
            if (aliveIds.Length > _filteredCache.Length) Array.Resize(ref _filteredCache, aliveIds.Length + 32);

            _filteredLength = 0;
            var inc = _includePools;
            var exc = _excludePools;
            var preds = _predicates;
            var iCnt = _includeCount;
            var eCnt = _excludeCount;
            var pCnt = _predicateCount;
            var pres = _presenter;

            foreach (var id in aliveIds)
            {
                bool match = true;

                for (int i = 0; i < iCnt; i++)
                {
                    if (!inc[i].Has(id)) { match = false; break; }
                }
                if (!match) continue;

                for (int i = 0; i < eCnt; i++)
                {
                    if (exc[i].Has(id)) { match = false; break; }
                }
                if (!match) continue;

                if (pCnt > 0)
                {
                    var entity = new EcsEntity(pres, id);
                    for (int i = 0; i < pCnt; i++)
                    {
                        if (!preds[i](entity)) { match = false; break; }
                    }
                }
                if (!match) continue;

                _filteredCache[_filteredLength++] = id;
            }
            _refWorld = EcsWorld.GetRefWorld();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Enumerator GetFastEnumerator()
        {
            if (_refWorld != EcsWorld.GetRefWorld()) RebuildCache();
            return new Enumerator(_presenter, _filteredCache, _filteredLength);
        }

        public Enumerator GetEnumerator() => GetFastEnumerator();

        public ref struct Enumerator
        {
            private readonly EcsPresenter _presenter;
            private readonly int[] _entities;
            private readonly int _count;
            private int _index;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator(EcsPresenter p, int[] entities, int count)
            {
                _presenter = p;
                _entities = entities;
                _count = count;
                _index = -1;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++_index < _count;

            public EcsEntity Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => new EcsEntity(_presenter, _entities[_index]);
            }
        }
    }

    public struct EcsFilter<T> where T : EcsPresenter, new()
    {
        private EcsFilter? _filter;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureInitialized()
        {
            if (_filter != null) return;
            _filter = new EcsFilter(EcsWorld.Get<T>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter Include<TComponent>() where TComponent : new()
        {
            EnsureInitialized();
            return _filter.Value.Include<TComponent>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter Include<TComponent>(Predicate<TComponent> predicate) where TComponent : new()
        {
            EnsureInitialized();
            return _filter.Value.Include(predicate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter Exclude<TComponent>() where TComponent : new()
        {
            EnsureInitialized();
            return _filter.Value.Exclude<TComponent>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter Where(Predicate<EcsEntity> predicate)
        {
            EnsureInitialized();
            return _filter.Value.Where(predicate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter WhereProvider<TComponent>(Predicate<TComponent> predicate) where TComponent : class, ILinkableProvider
        {
            EnsureInitialized();
            return _filter.Value.WhereProvider(predicate);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EcsFilter WhereProvider<TComponent>() where TComponent : class, ILinkableProvider
        {
            EnsureInitialized();
            return _filter.Value.WhereProvider<TComponent>();
        }

        public EcsFilter.Enumerator GetEnumerator()
        {
            EnsureInitialized();
            return _filter.Value.GetEnumerator();
        }
    }
}
