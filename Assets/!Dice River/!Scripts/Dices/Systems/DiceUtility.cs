using System.Collections.Generic;
using BitterECS.Core;
using UnityEngine;

public static class DiceUtility
{
    public static HashSet<Vector2Int> GetNeighbors(EcsEntity entity)
    {
        var gridDice = entity.Get<GridComponent>();
        var neighbors = entity.Get<NeighborsComponent>().neighbors;
        return gridDice.gridPresenter.GetNeighbors(gridDice.currentPosition, neighbors.ToArray());
    }
}
