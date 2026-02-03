using UnityEngine;
using System;
using BitterECS.Core;

public class PlayerLiftingSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Medium;

    private EcsEvent _ecsEvent =
        new EcsEvent<EntitiesPresenter>()
        .SubscribeWhereEntity<IsColliderHit>(e =>
        e.Has<GravityComponent>(c => !c.isGrounded),
        added: OnLifting);

    private static void OnLifting(EcsEntity entity)
    {
        var hitData = entity.Get<IsColliderHit>().hit;
        if (hitData.collider.gameObject.TryGetComponent<DiceProvider>(out _))
        {
            var provider = entity.GetProvider<EntitiesProvider>();
            var transform = provider.transform;

            var diceCollider = hitData.collider;
            var diceTopY = diceCollider.bounds.max.y;
            var offset = 0.5f;

            if (transform.TryGetComponent<Collider>(out var playerCollider))
            {
                offset = transform.position.y - playerCollider.bounds.min.y;
            }

            var diceCenter = diceCollider.bounds.center;
            var newPosition = transform.position;

            newPosition.x = diceCenter.x;
            newPosition.z = diceCenter.z;
            newPosition.y = diceTopY + offset;

            transform.position = newPosition;
        }
        entity.Remove<IsColliderHit>();
    }
}
