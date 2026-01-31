using System;
using System.Collections.Generic;
using BitterECS.Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceIntiRoleSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private EcsEvent _ecsEvent =
    new EcsEvent<DicePresenter>(Priority.High).Subscribe<RoleComponent>(OnRole);

    private EcsEvent _ecsEventProtect =
    new EcsEvent<DicePresenter>().SubscribeWhereEntity<RoleComponent>(e => e.Has<ProtectiveComponent>(), OnProtectingRole);

    private EcsEvent _ecsEventAdding =
    new EcsEvent<DicePresenter>().SubscribeWhereEntity<RoleComponent>(e => e.Has<AddingComponent>(), OnAddingRole);

    private EcsEvent _ecsEventReroll =
    new EcsEvent<DicePresenter>().SubscribeWhereEntity<RoleComponent>(e => e.Has<TagRerollDiceComponent>(), OnReroll);

    private static void OnReroll(EcsEntity entity)
    {
        var neighbors = GetNeighbors(entity);

        ref var gridDice = ref entity.Get<GridComponent>();

        foreach (var entityIndex in neighbors)
        {
            var entityToGrid = gridDice.gridPresenter.GetByIndex(entityIndex).Entity;
            OnRole(entityToGrid);
        }
    }

    private static void OnAddingRole(EcsEntity entity)
    {
        var neighbors = GetNeighbors(entity);
        var modification = entity.Get<AddingComponent>().addingModification;
        ref var gridDice = ref entity.Get<GridComponent>();

        foreach (var entityIndex in neighbors)
        {
            var entityToGrid = gridDice.gridPresenter.GetByIndex(entityIndex).Entity;
            ref var roleComponent = ref entityToGrid.Get<RoleComponent>();
            roleComponent.currentRole += modification;
        }
    }

    private static void OnProtectingRole(EcsEntity entity)
    {
        //TODO: Перекидывание урона
    }

    private static void OnRole(EcsEntity entity)
    {
        ref var roleComponent = ref entity.Get<RoleComponent>();
        roleComponent.currentRole = Random.Range(1, 6); //TODO: not usign random
    }

    private static HashSet<Vector2Int> GetNeighbors(EcsEntity entity)
    {
        ref var gridDice = ref entity.Get<GridComponent>();
        ref var neighbors = ref entity.Get<NeighborsComponent>().neighbors;
        return gridDice.gridPresenter.GetNeighbors(gridDice.currentPosition, neighbors.ToArray());
    }

}
