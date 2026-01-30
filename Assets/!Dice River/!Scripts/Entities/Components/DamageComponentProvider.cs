using UnityEngine;
using System;
using BitterECS.Integration;

[Serializable]
public struct DamageComponent
{
    public int damage;
}

public class DamageComponentProvider : ProviderEcs<DamageComponent>
{
}
