using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public class TagBombHazardColliderSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private EcsEvent _ecsEvent =
    new EcsEvent<DicePresenter>()
        .SubscribeWhere<IsTriggerColliderEnter>(c => c.entity.Has<TagBombHazard>(), added: OnDiceCollider);

    private static void OnDiceCollider(EcsEntity entity)
    {
        ref var gridComponent = ref entity.Get<GridComponent>();
        ref var collisionComponent = ref entity.Get<IsTriggerColliderEnter>();
        var damagingComponent = collisionComponent.entity.Get<DamageConstComponent>();
        collisionComponent.entity.Destroy();

        var positionGrid = gridComponent.currentPosition;
        var gridPresenter = gridComponent.gridPresenter;
        var neighbors = gridPresenter.GetSquaredNeighbors(positionGrid, e => e != null);
        foreach (var index in neighbors)
        {
            var isGet = gridPresenter.TryGetValue(index, out var provider);
            if (isGet)
            {
                ref var rollComponent = ref entity.Get<RollComponent>();
                rollComponent.value -= damagingComponent.damage;
                if (rollComponent.value <= 0)
                {
                    provider.Entity.AddFrameToEvent<IsDestroy>();
                }
                else
                {
                    entity.GetProvider<DiceProvider>().spriteRoll.Select(rollComponent.value);
                }
            }
        }

        ref var rollContact = ref entity.Get<RollComponent>();
        rollContact.value -= damagingComponent.damage;

        if (rollContact.value <= 0)
        {
            entity.AddFrameToEvent<IsDestroy>(new());
            return;
        }

        entity.GetProvider<DiceProvider>().spriteRoll.Select(rollContact.value);
    }
}
