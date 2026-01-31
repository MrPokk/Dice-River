using BitterECS.Integration;
using UnityEngine;

public class DiceSetterSystem
{
    public static void SpawnDiceRaft(Vector2Int index, DiceProvider prefab, out ProviderEcs instantiateObject)
    {
        var gridDice = Startup.GridWorld;
        if (!gridDice.IsWithinGrid(index))
        {
            gridDice.AddGridCell(index);
        }

        gridDice.InitializeGameObject(index, prefab, out instantiateObject, Startup.GridRaftParent.transform);
        instantiateObject.Entity.Add<RoleComponent>(new());
        instantiateObject.Entity.Add<GridComponent>(new(index, gridDice));
    }
}
