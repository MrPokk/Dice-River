using UnityEngine;

public class DiceRaftInitSystem
{
    public static void Initialize(GridConfig generationConfig, params DiceProvider[] prefabsDice)
    {
        var cellsGeneration = generationConfig.CellsWorld;

        foreach (var worldPos in cellsGeneration)
        {
            var dicePrefab = prefabsDice[Random.Range(0, prefabsDice.Length)];
            DicePlacingSystem.SpawnDiceRaft(worldPos, dicePrefab, out _);
        }
    }
}
