using BitterECS.Core;

public class TagRockHazardColliderSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private EcsEvent _ecsEvent =
    new EcsEvent<DicePresenter>()
        .SubscribeWhere<IsTriggerColliderEnter>(c => c.entityHit.Has<TagRockHazard>(), added: OnDiceCollider);

    private static void OnDiceCollider(EcsEntity entity)
    {
        ref var rollComponent = ref entity.Get<RollComponent>();
        ref var collisionComponent = ref entity.Get<IsTriggerColliderEnter>();
        var damagingComponent = collisionComponent.entityHit.Get<DamageConstComponent>();
        collisionComponent.entityHit.AddFrameToEvent<IsSoundPlay>();

        collisionComponent.entityHit.Destroy();
        rollComponent.value -= damagingComponent.damage;

        if (rollComponent.value <= 0)
        {
            entity.AddFrameToEvent<IsDestroy>(new());
            return;
        }
        entity.GetProvider<DiceProvider>().spriteRoll.Select(rollComponent.value);
        entity.AddFrameToEvent<IsTargetingEvent>();
    }
}
