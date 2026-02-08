using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public class UILinkingPrefabComponent
{
    public MonoBehaviour linkingPrefab;
}

public class UILinkingPrefabComponentProvider : ProviderEcs<UILinkingPrefabComponent>
{
    private void OnValidate()
    {
        if (_value.linkingPrefab == null)
        {
            Debug.LogError($"LinkingPrefab is null on {gameObject.name}");
        }
    }
}
