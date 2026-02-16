using BitterECS.Core;

public class TagDiceRerollSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private readonly EcsEvent _ecsEventAdding =
    new EcsEvent<DicePresenter>()
    .SubscribeWhereEntity<IsActivatingEvent>(e =>
        EcsConditions.Has<TagRerollDice, NeighborsComponent>(e), OnReroll);

    private static void OnReroll(EcsEntity entity)
    {
        var neighbors = DiceUtility.GetNeighbors(entity);
        var gridDice = entity.Get<GridComponent>();

        foreach (var entityIndex in neighbors)
        {
            var providerEcs = (DiceProvider)gridDice.gridPresenter.GetByIndex(entityIndex);
            var entityToGrid = providerEcs.Entity;

            if (providerEcs != null)
            {
                entityToGrid.AddFrameToEvent<IsTargetingEvent>();
                providerEcs.ReRolling();
            }
        }
    }
}
