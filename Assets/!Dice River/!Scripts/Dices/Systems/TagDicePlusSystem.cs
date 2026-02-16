using System;
using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public class TagDicePlusSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private readonly EcsEvent _ecsEventAdding =
    new EcsEvent<DicePresenter>()
    .SubscribeWhereEntity<IsActivatingEvent>(e =>
        EcsConditions.Has<TagPlusForward, NeighborsComponent>(e), removed: OnAdding);

    private static void OnAdding(EcsEntity entity)
    {
        var neighbors = DiceUtility.GetNeighbors(entity);
        var modification = entity.Get<TagPlusForward>().amount;
        var gridDice = entity.Get<GridComponent>();

        foreach (var entityIndex in neighbors)
        {
            var providerEcs = (DiceProvider)gridDice.gridPresenter.GetByIndex(entityIndex);
            var entityToGrid = providerEcs.Entity;
            ref var roleComponent = ref entityToGrid.Get<RollComponent>();

            var newValue = roleComponent.value + modification;
            roleComponent.value = ((newValue - 1) % 6 + 6) % 6 + 1;

            providerEcs.spriteRoll.Select(roleComponent.value);

            entityToGrid.AddFrameToEvent<IsActivatingEvent>();
            entityToGrid.AddFrameToEvent<IsTargetingEvent>();
        }
    }
}
