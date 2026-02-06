using System;
using BitterECS.Integration;

[Serializable]
public struct RollComponent
{
    [ReadOnly] public int currentRole;
}

public class RollComponentProvider : ProviderEcs<RollComponent>
{ }
