using BitterECS.Core;
using UnityEngine;

public class TagPouchPickupSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Medium;

    private EcsEvent _ecsEvent =
    new EcsEvent<EntitiesPresenter>()
        .SubscribeWhere<IsTriggerColliderEnter>(c => EcsConditions.Has<TagPouchPickup, DiceContainer>(c.entityHit), added: OnPickup);

    private static void OnPickup(EcsEntity entity)
    {
        var collision = entity.Get<IsTriggerColliderEnter>();
        var variableDices = collision.entityHit.Get<DiceContainer>().groups;
        var selectVariableDices = variableDices[Random.Range(0, variableDices.Count)];

        foreach (var dice in selectVariableDices.dice)
        {
            Startup.HandStackControllerDice.Add(dice.NewEntity(), dice.spriteIcon.Prefab());
        }

        collision.entityHit.Destroy();
    }
}
