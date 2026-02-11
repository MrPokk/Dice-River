using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct TagMultiplierArea
{
    public float minMultiplier;
    public float maxMultiplier;
}

[RequireComponent(typeof(NeighborsComponentProvider))]
public class TagMultiplierAreaProvider : ProviderEcs<TagMultiplierArea> { }
