using System;
using BitterECS.Integration.Unity;

[Serializable]
public struct TouchDamageComponent
{
    public int damage;
    public int layer;
}

public class TouchDamageComponentProvider : ProviderEcs<TouchDamageComponent>
{
}
