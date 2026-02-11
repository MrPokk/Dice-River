using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct TagPlusForward
{
    public int amount;
}

[RequireComponent(typeof(NeighborsComponentProvider))]
public class TagPlusForwardProvider : ProviderEcs<TagPlusForward> { }
