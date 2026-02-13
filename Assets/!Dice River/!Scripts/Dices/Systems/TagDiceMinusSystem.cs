using System;
using BitterECS.Core;
using BitterECS.Integration;

public class TagDiceMinusSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private readonly EcsEvent _ecsEventAdding =
    new EcsEvent<DicePresenter>()
    .SubscribeWhereEntity<IsInstantiateEvent>(e =>
        EcsConditions.Has<TagMinusForward, NeighborsComponent>(e), removed: OnAdding);

    private static void OnAdding(EcsEntity entity)
    {
        var neighbors = DiceUtility.GetNeighbors(entity);
        var modification = entity.Get<TagMinusForward>().amount;
        var gridDice = entity.Get<GridComponent>();

        if (modification > 0)
        {
            modification *= -1;
        }

        foreach (var entityIndex in neighbors)
        {
            var providerEcs = (DiceProvider)gridDice.gridPresenter.GetByIndex(entityIndex);
            var entityToGrid = providerEcs.Entity;
            ref var roleComponent = ref entityToGrid.Get<RollComponent>();

            var rawValue = roleComponent.value + modification;
            roleComponent.value = ((rawValue - 1) % 6 + 6) % 6 + 1;

            providerEcs.spriteRoll.Select(roleComponent.value);
            entityToGrid.AddFrameToEvent<IsInstantiateEvent>();
        }
    }
}
