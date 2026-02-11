using System;
using BitterECS.Core;

public class DiceTagMinusSystem : IEcsAutoImplement
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

        foreach (var entityIndex in neighbors)
        {
            var providerEcs = (DiceProvider)gridDice.gridPresenter.GetByIndex(entityIndex);
            var entityToGrid = providerEcs.Entity;
            ref var roleComponent = ref entityToGrid.Get<RollComponent>();
            roleComponent.value -= modification;
            providerEcs.spriteRoll.Select(roleComponent.value);
        }
    }
}
