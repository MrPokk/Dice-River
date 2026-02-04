using BitterECS.Integration;
using UnityEngine;

public class DiceSetterSystem
{
    private static MonoGridPresenter GridDice => Startup.GridRaft.monoGrid;
    private static GameObject GridDiceGameObject => Startup.GridRaft.gridParent;
    public static void SpawnDiceRaft(Vector2Int index, DiceProvider prefab, out ProviderEcs instantiateObject)
    {
        var isSet = GridDice.InitializeGameObject(index, prefab, out instantiateObject, GridDiceGameObject.transform);
        if (!isSet)
        {
            return;
        }
        instantiateObject.Entity.Add<RoleComponent>(new());
        instantiateObject.Entity.Add<GridComponent>(new(index, GridDice));
    }

    public static void SpawnDiceRaft(Vector3 indexWorld, DiceProvider prefab, out ProviderEcs instantiateObject)
    {
        SpawnDiceRaft(GridDice.ConvertingPosition(indexWorld), prefab, out instantiateObject);
    }
}
