using BitterECS.Core;
using UnityEngine;

public class TagRockHazardColliderSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private EcsEvent _ecsEvent =
    new EcsEvent<DicePresenter>()
        .SubscribeWhere<IsTriggerColliderEnter>(c => c.entity.Has<TagRockHazard>(), added: OnDiceCollider);

    private static void OnDiceCollider(EcsEntity entity)
    {
        Debug.Log(entity);
        ref var rollComponent = ref entity.Get<RollComponent>();
        ref var collisionComponent = ref entity.Get<IsTriggerColliderEnter>();
        collisionComponent.entity.Destroy();
        rollComponent.value--;

        if (rollComponent.value <= 0)
        {
            entity.AddFrameToEvent<IsDestroy>(new());
            return;
        }


        entity.GetProvider<DiceProvider>().spriteRoll.Select(rollComponent.value);
    }
}
