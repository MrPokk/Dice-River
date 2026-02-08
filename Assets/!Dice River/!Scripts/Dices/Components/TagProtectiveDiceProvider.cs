using System;
using BitterECS.Integration;

[Serializable]
public struct TagProtectiveDice
{
    public int region;
}

public class TagProtectiveDiceProvider : ProviderEcs<TagProtectiveDice> { }
