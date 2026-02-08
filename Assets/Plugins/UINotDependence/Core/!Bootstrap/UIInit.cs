using System;
using System.Collections.Generic;
using UnityEngine;

namespace UINotDependence.Core
{
    public class UIInit
    {
        private static readonly string s_uiPrefabsPath = "UI";

        public static void Initialize()
        {
            var allBinders = new Dictionary<Type, WindowBinder>();
            var allPrefabs = Resources.LoadAll<GameObject>(s_uiPrefabsPath);

            foreach (var prefab in allPrefabs)
            {
                if (prefab.TryGetComponent<WindowBinder>(out var binder))
                {
                    var type = binder.GetType();
                    allBinders.TryAdd(type, binder);
                }
            }

            UIFactory.Create(allBinders);
        }
    }
}
