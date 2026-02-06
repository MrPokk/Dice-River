using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

public static class WeightedRandomUtility
{
    public static ProviderEcs GetWeighted(List<WeightedPrefab> list)
    {
        if (list == null || list.Count == 0) return null;

        var totalWeight = 0f;
        foreach (var item in list) totalWeight += item.weight;

        if (totalWeight <= 0f) return list[Random.Range(0, list.Count)].prefab;

        var randomValue = Random.Range(0, totalWeight);
        var currentWeight = 0f;

        foreach (var item in list)
        {
            currentWeight += item.weight;
            if (randomValue <= currentWeight) return item.prefab;
        }

        return list[0].prefab;
    }
}
