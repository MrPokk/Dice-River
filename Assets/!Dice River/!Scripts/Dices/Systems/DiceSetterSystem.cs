using BitterECS.Integration;
using UnityEngine;

public class DiceSetterSystem
{
    public static void SpawnDiceRaft(Vector2Int index, DiceProvider prefab, out ProviderEcs instantiateObject)
    {
        var gridDice = Startup.GridRaft;

        var isSet = gridDice.InitializeGameObject(index, prefab, out instantiateObject, DiceRaftInitSystem.GridRaftParent.transform);
        if (!isSet)
        {
            return;
        }
        instantiateObject.Entity.Add<RoleComponent>(new());
        instantiateObject.Entity.Add<GridComponent>(new(index, gridDice));
    }
}
