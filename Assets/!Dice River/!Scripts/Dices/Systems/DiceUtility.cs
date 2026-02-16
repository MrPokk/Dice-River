using System.Collections.Generic;
using BitterECS.Core;
using UnityEngine;

public static class DiceUtility
{
    public static HashSet<Vector2Int> GetNeighbors(EcsEntity entity)
    {
        var gridDice = entity.Get<GridComponent>();
        ref var neighbors = ref entity.Get<NeighborsComponent>().neighbors;
        if (neighbors == null) throw new("NeighborsComponent is neighbors is null");
        return gridDice.gridPresenter.GetNeighbors(gridDice.currentPosition, neighbors.ToArray(), e => e != null);
    }
}
