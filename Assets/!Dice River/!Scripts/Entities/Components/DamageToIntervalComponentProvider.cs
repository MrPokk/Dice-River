using UnityEngine;
using System;
using BitterECS.Integration;

[Serializable]
public struct DamageToIntervalComponent
{
    public int damage;
    public float damageIntervalSecond;
}

public class DamageToIntervalComponentProvider : ProviderEcs<DamageToIntervalComponent>
{
}
