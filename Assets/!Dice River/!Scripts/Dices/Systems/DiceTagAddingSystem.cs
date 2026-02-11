using BitterECS.Core;

public class DiceTagAddingSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private readonly EcsEvent _ecsEventAdding =
    new EcsEvent<DicePresenter>()
    .SubscribeWhereEntity<IsRollingProcess>(e =>
        EcsConditions.Has<TagAddingDice, NeighborsComponent>(e), removed: OnAdding);

    private static void OnAdding(EcsEntity entity)
    {
        var neighbors = DiceUtility.GetNeighbors(entity);
        var modification = entity.Get<TagAddingDice>().addingModification;
        var gridDice = entity.Get<GridComponent>();

        foreach (var entityIndex in neighbors)
        {
            var entityToGrid = gridDice.gridPresenter.GetByIndex(entityIndex).Entity;
            ref var roleComponent = ref entityToGrid.Get<RollComponent>();
            roleComponent.value += modification;
        }
    }
}
