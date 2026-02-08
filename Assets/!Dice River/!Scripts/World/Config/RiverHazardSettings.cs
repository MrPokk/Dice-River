using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

[CreateAssetMenu(fileName = "RiverHazardSettings", menuName = "Settings/RiverHazard")]
public class RiverHazardSettings : ScriptableObject
{
    [Range(0f, 1f)]
    public float hazardChance = 0.1f;
    public List<WeightedPrefab> hazard;

    public ProviderEcs GetRandomHazard() => WeightedRandomUtility.GetWeighted(hazard);
}
