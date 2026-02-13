using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

[CreateAssetMenu(fileName = "RiverPickupSettings", menuName = "Settings/RiverPickup")]
public class RiverPickupSettings : ScriptableObject
{
    [Range(0f, 1f)]
    public float pickupChance = 0.1f;
    public List<WeightedPrefab> pickups;

    public ProviderEcs GetRandom() => WeightedRandomUtility.GetWeighted(pickups);
}
