using System;
using BitterECS.Core;

public class DiceTagRerollSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private readonly EcsEvent _ecsEventAdding =
    new EcsEvent<DicePresenter>()
    .SubscribeWhereEntity<IsRollingProcess>(e =>
        EcsConditions.Has<TagRerollDice, NeighborsComponent>(e), OnReroll);

    private static void OnReroll(EcsEntity entity)
    {
        var neighbors = DiceUtility.GetNeighbors(entity);

        ref var gridDice = ref entity.Get<GridComponent>();

        foreach (var entityIndex in neighbors)
        {
            var providerEcs = gridDice.gridPresenter.GetByIndex(entityIndex);
            var entityToGrid = providerEcs.Entity.GetProvider<DiceProvider>();
            entityToGrid.ReRolling();
        }
    }
}
