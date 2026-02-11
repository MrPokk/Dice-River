using BitterECS.Core;

public class TagRockHazardColliderSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private EcsEvent _ecsEvent =
    new EcsEvent<DicePresenter>()
        .SubscribeWhere<IsTriggerColliderEnter>(c => c.entity.Has<TagRockHazard>(), added: OnDiceCollider);

    private static void OnDiceCollider(EcsEntity entity)
    {
        ref var rollComponent = ref entity.Get<RollComponent>();
        ref var collisionComponent = ref entity.Get<IsTriggerColliderEnter>();
        var damagingComponent = collisionComponent.entity.Get<DamageConstComponent>();
        collisionComponent.entity.Destroy();
        rollComponent.value -= damagingComponent.damage;

        if (rollComponent.value <= 0)
        {
            entity.AddFrameToEvent<IsDestroy>(new());
            return;
        }

        entity.GetProvider<DiceProvider>().spriteRoll.Select(rollComponent.value);
    }
}
