using System;
using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

public class DiceInteractionSystem
{
    private static MonoGridPresenter GridDice => Startup.GridRaft.monoGrid;
    private static GameObject GridDiceGameObject => Startup.GridRaft.gridParent;

    private static readonly List<Vector2Int> _cachedActiveIndices = new(32);

    private static readonly Dictionary<Vector2Int, Action<DiceProvider, DiceProvider, bool>> s_renderingActions = new()
    {
        { Vector2Int.left, (curr, neigh, connect) => {
            curr.spriteSide.SetLeftActive(!connect); neigh.spriteSide.SetRightActive(!connect); } },
        { Vector2Int.right, (curr, neigh, connect) => {
            curr.spriteSide.SetRightActive(!connect); neigh.spriteSide.SetLeftActive(!connect); } },
        { Vector2Int.up, (curr, neigh, connect) =>
            neigh.spriteSide.SetFrontActive(!connect) },
        { Vector2Int.down, (curr, neigh, connect) =>
            curr.spriteSide.SetFrontActive(!connect) }
    };

    public static List<Vector2Int> GetActiveDiceIndices()
    {
        _cachedActiveIndices.Clear();

        var gridDict = GridDice.GetGridDictionary();

        foreach (var kvp in gridDict)
        {
            if (kvp.Value is DiceProvider)
            {
                _cachedActiveIndices.Add(kvp.Key);
            }
        }

        return _cachedActiveIndices;
    }

    public static DiceProvider Extraction(Vector2Int index)
    {
        var objectExtract = GridDice.GetGameObject(index) as DiceProvider;
        if (objectExtract == null) return null;

        UpdateNeighbors(index, objectExtract, false);

        GridDice.ExtractGameObject(index);

        objectExtract.spriteSide.ResetSides();
        objectExtract.Entity.AddFrameToEvent<IsExtractionEvent>();

        return objectExtract;
    }

    public static DiceProvider Extraction(Vector3 indexWorld)
    {
        return Extraction(GridDice.ConvertingPosition(indexWorld));
    }

    public static bool Placing(Vector2Int index, DiceProvider diceObject)
    {
        if (diceObject == null) return false;

        var isSet = GridDice.TrySetGameObject(index, diceObject);
        if (!isSet) return false;

        diceObject.spriteSide.ResetSides();

        diceObject.Entity.Add<GridComponent>(new(index, GridDice));
        diceObject.Entity.AddFrameToEvent<IsPlacingEvent>();

        UpdateNeighbors(index, diceObject, true);

        return true;
    }

    public static bool Placing(Vector3 indexWorld, DiceProvider diceObject)
    {
        return Placing(GridDice.ConvertingPosition(indexWorld), diceObject);
    }

    public static bool IsPlacing(Vector2Int index)
    {
        return GridDice.HasNotGameObject(index) && GridDice.IsWithinGrid(index);
    }

    public static bool IsPlacing(Vector3 indexWorld)
    {
        return IsPlacing(GridDice.ConvertingPosition(indexWorld));
    }

    public static void InstantiateObject(Vector2Int index, DiceProvider prefab, out ProviderEcs instantiateObject)
    {
        var isSet = GridDice.InitializeGameObject(index, prefab, out instantiateObject, GridDiceGameObject.transform);
        if (!isSet) return;

        instantiateObject.Entity.Add<GridComponent>(new(index, GridDice));

        if (instantiateObject is DiceProvider dice)
        {
            dice.spriteSide.ResetSides();
            UpdateNeighbors(index, dice, true);
            instantiateObject.Entity.AddFrameToEvent<IsActivatingEvent>();
        }
    }

    public static void InstantiateObject(Vector3 indexWorld, DiceProvider prefab, out ProviderEcs instantiateObject)
    {
        InstantiateObject(GridDice.ConvertingPosition(indexWorld), prefab, out instantiateObject);
    }

    private static void UpdateNeighbors(Vector2Int index, DiceProvider currentDice, bool isConnecting)
    {
        if (currentDice == null || currentDice.spriteSide == null) return;

        var offsets = new Vector2Int[] { Vector2Int.down, Vector2Int.up, Vector2Int.left, Vector2Int.right };
        var foundNeighbors = GridDice.GetNeighbors(index, offsets, entity => entity != null);

        foreach (var neighborPos in foundNeighbors)
        {
            if (GridDice.TryGetValue(neighborPos, out var neighborProvider) &&
                neighborProvider is DiceProvider neighborDice &&
                s_renderingActions.TryGetValue(neighborPos - index, out var action))
            {
                action(currentDice, neighborDice, isConnecting);
            }
        }
    }
}
