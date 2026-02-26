using System;
using BitterECS.Integration.Unity;

[Serializable]
public struct RollComponent
{
    [ReadOnly] public int value;
}

public class RollComponentProvider : ProviderEcs<RollComponent>
{ }
