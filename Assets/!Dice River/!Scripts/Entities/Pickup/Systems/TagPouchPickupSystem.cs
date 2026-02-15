using System.Linq;
using BitterECS.Core;
using UnityEngine;

public class TagPouchPickupSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Medium;

    private EcsEvent _ecsEvent =
    new EcsEvent<DicePresenter>()
        .SubscribeWhere<IsTriggerColliderEnter>(c => EcsConditions.Has<TagPouchPickup, DiceContainer>(c.entityHit), added: OnPickup);

    private EcsEvent _ecsEventPlayer =
    new EcsEvent<EntitiesPresenter>()
        .SubscribeWhere<IsTriggerColliderEnter>(c => EcsConditions.Has<TagPouchPickup, DiceContainer>(c.entityHit), added: OnPickup);

    private static void OnPickup(EcsEntity entity)
    {
        var collision = entity.Get<IsTriggerColliderEnter>();
        var currentTier = StartupGameplay.GState.currentDifficulty;

        var validGroups = collision.entityHit.Get<DiceContainer>().groups
            .Where(group => group.level <= currentTier)
            .ToList();

        if (validGroups.Count > 0)
        {
            var selectedGroup = validGroups[Random.Range(0, validGroups.Count)];

            foreach (var dice in selectedGroup.dice)
            {
                Startup.HandStackControllerDice.Add(dice.NewEntity(), dice.spriteIcon.Prefab());
            }
        }

        collision.entityHit.Destroy();
    }
}
