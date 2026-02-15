using System;
using System.Collections.Generic;

namespace BitterECS.Core
{
    public class EcsEventPool<T> : EcsPool<T> where T : new()
    {
        private IEcsEvent[] _subscriptions = new IEcsEvent[EcsConfig.InitialEventPoolCapacity];
        private int _subscriptionsCount = 0;

        private readonly Stack<IEcsEvent> _unSubscriptions = new();
        private readonly IComparer<IEcsEvent> _comparer = PriorityUtility.Sort();

        public void Subscribe(IEcsEvent eventTo)
        {
            for (var i = 0; i < _subscriptionsCount; i++)
            {
                if (_subscriptions[i] == eventTo) return;
            }

            if (_subscriptionsCount == _subscriptions.Length)
            {
                Array.Resize(ref _subscriptions, _subscriptionsCount * 2);
            }

            _subscriptions[_subscriptionsCount++] = eventTo;

            Array.Sort(_subscriptions, 0, _subscriptionsCount, _comparer);
        }

        public void Unsubscribe(IEcsEvent eventTo) => _unSubscriptions.Push(eventTo);

        public override void Add(int entityId, in T component)
        {
            base.Add(entityId, in component);

            for (var i = 0; i < _subscriptionsCount; i++)
            {
                var sub = _subscriptions[i];
                sub.Added?.Invoke(sub.Presenter.Get(entityId));
            }

            UnsubscribeRefresh();
        }

        public override void Remove(int entityId)
        {
            base.Remove(entityId);

            for (var i = 0; i < _subscriptionsCount; i++)
            {
                var sub = _subscriptions[i];
                sub.Removed?.Invoke(sub.Presenter.Get(entityId));
            }

            UnsubscribeRefresh();
        }

        public void UnsubscribeRefresh()
        {
            var unSubCount = _unSubscriptions.Count;
            if (unSubCount == 0) return;

            if (unSubCount == 1)
            {
                var target = _unSubscriptions.Pop();
                for (var i = 0; i < _subscriptionsCount; i++)
                {
                    if (_subscriptions[i] == target)
                    {
                        var itemsToMove = _subscriptionsCount - i - 1;
                        if (itemsToMove > 0)
                        {
                            Array.Copy(_subscriptions, i + 1, _subscriptions, i, itemsToMove);
                        }
                        _subscriptions[--_subscriptionsCount] = null;
                        break;
                    }
                }
                return;
            }

            var writeIndex = 0;
            for (var readIndex = 0; readIndex < _subscriptionsCount; readIndex++)
            {
                var currentItem = _subscriptions[readIndex];

                if (!_unSubscriptions.Contains(currentItem))
                {
                    if (writeIndex != readIndex)
                    {
                        _subscriptions[writeIndex] = currentItem;
                    }
                    writeIndex++;
                }
            }

            for (var i = writeIndex; i < _subscriptionsCount; i++)
            {
                _subscriptions[i] = null;
            }

            _subscriptionsCount = writeIndex;
            _unSubscriptions.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();
            for (var i = 0; i < _subscriptionsCount; i++) _subscriptions[i] = null;
            _subscriptionsCount = 0;
            _unSubscriptions.Clear();
        }
    }
}
