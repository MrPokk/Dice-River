using System;
using BitterECS.Integration.Unity;
using UnityEngine;

[Serializable]
public struct TagProtectiveDice
{
    public int region;
}

[RequireComponent(typeof(NeighborsComponentProvider))]
public class TagProtectiveDiceProvider : ProviderEcs<TagProtectiveDice> { }
