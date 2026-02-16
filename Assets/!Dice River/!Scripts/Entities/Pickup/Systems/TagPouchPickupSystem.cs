using System.Linq;
using BitterECS.Core;
using UnityEngine;

public class TagPouchPickupSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Medium;

    private EcsEvent _ecsEvent = new EcsEvent<DicePresenter>()
        .SubscribeWhere<IsTriggerColliderEnter>(c => EcsConditions.Has<TagPouchPickup, DiceContainer>(c.entityHit), added: OnPickup);

    private EcsEvent _ecsEventPlayer = new EcsEvent<EntitiesPresenter>()
        .SubscribeWhere<IsTriggerColliderEnter>(c => EcsConditions.Has<TagPouchPickup, DiceContainer>(c.entityHit), added: OnPickup);

    private static void OnPickup(EcsEntity entity)
    {
        var collision = entity.Get<IsTriggerColliderEnter>();
        var pouchEntity = collision.entityHit;

        var currentTier = StartupGameplay.GState.currentDifficulty;
        var containerComponent = pouchEntity.Get<DiceContainer>();

        if (containerComponent.settings == null || containerComponent.settings.Count == 0)
        {
            pouchEntity.Destroy();
            return;
        }

        var tierValue = (int)currentTier;
        var roll = Random.value;

        if (roll < containerComponent.tierUnderChance) tierValue--;
        else if (roll > containerComponent.tierOverChance) tierValue++;

        tierValue = Mathf.Clamp(tierValue, 0, (int)DifficultyTier.Tier3_Base);
        var targetTier = (DifficultyTier)tierValue;

        var validContainers = containerComponent.settings
            .Where(so => so != null && so.AverageDifficulty == targetTier)
            .ToList();

        if (validContainers.Count == 0)
        {
            validContainers = containerComponent.settings
                .Where(so => so != null && so.AverageDifficulty <= currentTier)
                .ToList();
        }

        if (validContainers.Count > 0)
        {
            var selectedSO = validContainers[Random.Range(0, validContainers.Count)];

            var possibleGroups = selectedSO.groups
                .Where(g => g != null && (int)g.level <= (int)targetTier + 1)
                .ToList();

            if (possibleGroups.Count > 0)
            {
                var selectedGroup = possibleGroups[Random.Range(0, possibleGroups.Count)];

                foreach (var diceProvider in selectedGroup.dice)
                {
                    if (diceProvider == null) continue;

                    Startup.HandStackControllerDice.Add(
                        diceProvider.NewEntity(),
                        diceProvider.spriteIcon.Prefab()
                    );
                }
            }
            else
            {
                Debug.LogWarning($"Container {selectedSO.name} has no groups for target tier {targetTier}");
            }
        }
        else
        {
            Debug.LogWarning("No suitable DiceContainerSO found even with offset logic!");
        }

        pouchEntity.Destroy();
    }
}
