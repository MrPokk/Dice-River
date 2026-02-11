using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct TagMinusForward
{
    public int amount;
}

[RequireComponent(typeof(NeighborsComponentProvider))]
public class TagMinusForwardProvider : ProviderEcs<TagMinusForward> { }
