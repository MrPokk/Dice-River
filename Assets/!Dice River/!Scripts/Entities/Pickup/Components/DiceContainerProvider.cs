using System;
using System.Collections.Generic;
using BitterECS.Integration;

[Serializable]
public struct DiceContainer
{
    public List<DiceGroup> groups;
}

[Serializable]
public struct DiceGroup
{
    public List<DiceProvider> dice;
}

public class DiceContainerProvider : ProviderEcs<DiceContainer> { }
