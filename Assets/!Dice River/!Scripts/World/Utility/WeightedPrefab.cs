using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public class WeightedPrefab
{
    public ProviderEcs prefab;
    [Range(0, 1)] public float weight = 0;
}
