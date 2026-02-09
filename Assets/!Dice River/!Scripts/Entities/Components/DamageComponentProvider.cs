using UnityEngine;
using System;
using BitterECS.Integration;

[Serializable]
public struct DamageComponent
{
    public int damage;
    public float damageIntervalSecond;
}

public class DamageComponentProvider : ProviderEcs<DamageComponent>
{
}
