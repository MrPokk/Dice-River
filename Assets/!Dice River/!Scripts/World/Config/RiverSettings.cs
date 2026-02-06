using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

[CreateAssetMenu(fileName = "RiverSettings", menuName = "Settings/RiverGlobal")]
public class RiverSettings : ScriptableObject
{
    public List<WeightedPrefab> shoreBasePrefabs;
    public RiverDecorationSettings decorationSettings;
    public List<WeightedPrefab> hazardSpawns;
    public List<WeightedPrefab> pickupSpawns;

    public ProviderEcs GetRandomShore() => WeightedRandomUtility.GetWeighted(shoreBasePrefabs);
    public ProviderEcs GetRandomHazard() => WeightedRandomUtility.GetWeighted(hazardSpawns);
    public ProviderEcs GetRandomPickup() => WeightedRandomUtility.GetWeighted(pickupSpawns);
}
