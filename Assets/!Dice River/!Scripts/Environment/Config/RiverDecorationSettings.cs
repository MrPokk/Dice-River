using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

[CreateAssetMenu(fileName = "RiverDecorationSettings", menuName = "Settings/RiverDecoration")]
public class RiverDecorationSettings : ScriptableObject
{
    public List<WeightedPrefab> trees;
    public List<WeightedPrefab> rocks;
    public List<WeightedPrefab> shadows;
    public List<WeightedPrefab> grass;

    public ProviderEcs GetRandomTree() => WeightedRandomUtility.GetWeighted(trees);
    public ProviderEcs GetRandomRock() => WeightedRandomUtility.GetWeighted(rocks);
    public ProviderEcs GetRandomShadow() => WeightedRandomUtility.GetWeighted(shadows);
    public ProviderEcs GetRandomGrass() => WeightedRandomUtility.GetWeighted(grass);
}
