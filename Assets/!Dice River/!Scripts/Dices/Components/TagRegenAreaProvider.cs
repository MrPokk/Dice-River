using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct TagRegenArea
{
    public float tickInterval;
    public int regenAmount;
}

[RequireComponent(typeof(NeighborsComponentProvider))]
public class TagRegenAreaProvider : ProviderEcs<TagRegenArea> { }
