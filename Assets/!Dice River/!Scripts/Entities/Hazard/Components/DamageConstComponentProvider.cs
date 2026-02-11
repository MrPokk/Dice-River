using UnityEngine;
using System;
using BitterECS.Integration;

[Serializable]
public struct DamageConstComponent
{
    public int damage;
}

public class DamageConstComponentProvider : ProviderEcs<DamageConstComponent>
{
}
