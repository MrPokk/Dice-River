using UnityEngine;

public class DiceRaftInitSystem
{
    public static void Initialize(GridConfig generationConfig, params DiceProvider[] prefabsDice)
    {
        var cellsGeneration = generationConfig.CellsWorld;

        foreach (var worldPos in cellsGeneration)
        {
            var dicePrefab = prefabsDice[Random.Range(0, prefabsDice.Length)];
            DiceInteractionSystem.InstantiateObject(worldPos, dicePrefab, out _);
        }
    }
}
