using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

[CreateAssetMenu(fileName = "RiverSettings", menuName = "Settings/RiverGlobal")]
public class RiverSettings : ScriptableObject
{
    public RiverDecorationSettings decorationSettings;
    public RiverHazardSettings hazardSettings;
    public List<WeightedPrefab> pickupSpawns;

    public ProviderEcs GetRandomPickup() => WeightedRandomUtility.GetWeighted(pickupSpawns);
}
