using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

public struct NeighborsComponent
{
    public List<Vector2Int> neighbors;
}

public class NeighborsComponentProvider : ProviderEcs<NeighborsComponent>
{

}
