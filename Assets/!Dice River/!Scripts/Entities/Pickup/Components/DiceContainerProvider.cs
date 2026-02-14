using System;
using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct DiceContainer
{
    public List<DiceGroup> groups;
}

[Serializable]
public struct DiceGroup
{
    [Tooltip("Level at which this group of dice will be available")]
    public DifficultyTier level;

    public List<DiceProvider> dice;
}

public class DiceContainerProvider : ProviderEcs<DiceContainer> { }
