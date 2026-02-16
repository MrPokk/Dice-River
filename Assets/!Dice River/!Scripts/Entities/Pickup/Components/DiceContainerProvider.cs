using System;
using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct DiceContainer
{
    [Range(0, 1f)] public float tierUnderChance;
    [Range(0, 1f)] public float tierOverChance;
    public List<DiceContainerSO> settings;
}

public class DiceContainerProvider : ProviderEcs<DiceContainer> { }
