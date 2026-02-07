
#if VCONTAINER_AVAILABLE

using System;
using UnityEngine;

namespace UIFeture.Core
{
    public interface IUIProvider
    {
        public GameObject UIObject { get; }

        void AddListener(Action actionClick);
        void RemoveListener(Action actionClick);
        void SetSelectNeighbors(GameObject upNeighbour,
                                 GameObject downNeighbour,
                                 GameObject leftNeighbour,
                                 GameObject rightNeighbour);
        void Refresh();
    }
}
#endif
