using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BitterECS.Core
{
    public class EcsEventPool<T> : EcsPool<T> where T : new()
    {
        private readonly SortedSet<IEcsEvent> _subscriptions = new(PriorityUtility.Sort());
        private readonly Stack<IEcsEvent> _unSubscriptions = new();

        public void Subscribe(IEcsEvent eventTo) => _subscriptions.Add(eventTo);
        public void Unsubscribe(IEcsEvent eventTo) => _unSubscriptions.Push(eventTo);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Add(int entityId, in T component)
        {
            base.Add(entityId, in component);
            foreach (var subscription in _subscriptions)
            {
                subscription.Added?.Invoke(subscription.Presenter.Get(entityId));
            }
            UnsubscribeRefresh();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Remove(int entityId)
        {
            base.Remove(entityId);
            foreach (var subscription in _subscriptions)
            {
                subscription.Removed?.Invoke(subscription.Presenter.Get(entityId));
            }
            UnsubscribeRefresh();
        }

        public void UnsubscribeRefresh()
        {
            foreach (var subscriptions in _unSubscriptions)
            {
                _subscriptions.Remove(subscriptions);
            }
            _unSubscriptions.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();
            _subscriptions.Clear();
        }
    }
}
