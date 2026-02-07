using BitterECS.Integration;
using UnityEngine;
using System.Collections.Generic;
using System;

public class DicePlacingSystem
{
    private static MonoGridPresenter GridDice => Startup.GridRaft.monoGrid;
    private static GameObject GridDiceGameObject => Startup.GridRaft.gridParent;

    private static readonly Dictionary<Vector2Int, Action<DiceProvider, DiceProvider>> s_renderingActions = new()
    {
        { Vector2Int.left, (curr, neigh) => { curr.spriteSide.ToggleLeft(); neigh.spriteSide.ToggleRight(); } },
        { Vector2Int.right, (curr, neigh) => { curr.spriteSide.ToggleRight(); neigh.spriteSide.ToggleLeft(); } },
        { Vector2Int.up, (curr, neigh) => neigh.spriteSide.ToggleFront() },
        { Vector2Int.down, (curr, neigh) => curr.spriteSide.ToggleFront() }
    };

    public static void SpawnDiceRaft(Vector2Int index, DiceProvider prefab, out ProviderEcs instantiateObject)
    {
        var isSet = GridDice.InitializeGameObject(index, prefab, out instantiateObject, GridDiceGameObject.transform);
        if (!isSet) return;

        instantiateObject.Entity.AddOrReplace<GridComponent>(new(index, GridDice));
        OptimizedRendering(index, instantiateObject);
    }

    public static void SpawnDiceRaft(Vector3 indexWorld, DiceProvider prefab, out ProviderEcs instantiateObject)
    {
        SpawnDiceRaft(GridDice.ConvertingPosition(indexWorld), prefab, out instantiateObject);
    }

    private static void OptimizedRendering(Vector2Int index, ProviderEcs instantiateObject)
    {
        if (instantiateObject is not DiceProvider currentDice || currentDice.spriteSide == null) return;

        var offsets = new Vector2Int[] { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };
        var foundNeighbors = GridDice.GetNeighbors(index, offsets, entity => entity != null);

        foreach (var neighborPos in foundNeighbors)
        {
            if (GridDice.TryGetValue(neighborPos, out var neighborProvider) &&
                neighborProvider is DiceProvider neighborDice &&
                s_renderingActions.TryGetValue(neighborPos - index, out var action))
            {
                action(currentDice, neighborDice);
            }
        }
    }
}
