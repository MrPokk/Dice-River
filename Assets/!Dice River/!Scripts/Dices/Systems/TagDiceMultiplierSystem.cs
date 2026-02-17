using System;
using BitterECS.Core;
using Random = UnityEngine.Random;

public class TagDiceMultiplierSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private readonly EcsEvent _ecsEventAdding =
    new EcsEvent<DicePresenter>()
    .SubscribeWhereEntity<IsActivatingEvent>(e =>
        EcsConditions.Has<TagMultiplierArea, NeighborsComponent>(e), removed: OnAdding);

    private static void OnAdding(EcsEntity entity)
    {
        var neighbors = DiceUtility.GetNeighbors(entity);
        var tagMultiplierArea = entity.Get<TagMultiplierArea>();
        var minAmount = tagMultiplierArea.minMultiplier;
        var maxAmount = tagMultiplierArea.maxMultiplier;

        var modification = (int)Random.Range(minAmount, maxAmount + 1);

        var gridDice = entity.Get<GridComponent>();

        foreach (var entityIndex in neighbors)
        {
            var providerEcs = (DiceProvider)gridDice.gridPresenter.GetByIndex(entityIndex);
            var entityToGrid = providerEcs.Entity;
            ref var roleComponent = ref entityToGrid.Get<RollComponent>();

            var baseValue = roleComponent.value == 1 ? roleComponent.value + 1 : roleComponent.value;
            var value = baseValue * modification;
            var modificationValue = Math.Clamp(value, 1, 6);
            roleComponent.value = Math.Abs(modificationValue);

            providerEcs.spriteRoll.Select(roleComponent.value);

            entityToGrid.AddFrameToEvent<IsTargetingEvent>();
        }
    }
}
