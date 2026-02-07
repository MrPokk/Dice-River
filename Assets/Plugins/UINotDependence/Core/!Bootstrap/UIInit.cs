using System;
using System.Collections.Generic;
using UnityEngine;

namespace UINotDependence.Core
{
    public class UIInit
    {
        private readonly string _uiPrefabsPath = "UI";

        public void Initialize()
        {
            var allBinders = new Dictionary<Type, WindowBinder>();
            var allPrefabs = Resources.LoadAll<GameObject>(_uiPrefabsPath);

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
