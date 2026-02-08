using System;
using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public class NeighborsComponent
{
    public List<Vector2Int> neighbors;
}

public class NeighborsComponentProvider : ProviderEcs<NeighborsComponent>
{ }
