using UnityEngine;
using System;
using BitterECS.Core;

public class PlayerLiftingSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

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

            if (transform.TryGetComponent<CharacterController>(out var controller))
            {
                var diceCollider = hitData.collider;
                var diceTopY = diceCollider.bounds.max.y;

                var footOffset = controller.height / 2f;
                var targetY = diceTopY + footOffset + controller.skinWidth;

                if (targetY > transform.position.y)
                {
                    var diceCenter = diceCollider.bounds.center;
                    Vector3 newPosition = new Vector3(diceCenter.x, targetY, diceCenter.z);

                    controller.enabled = false;
                    transform.position = newPosition;
                    controller.enabled = true;

                    if (entity.Has<GravityComponent>())
                    {
                        var gravity = entity.Get<GravityComponent>();
                        gravity.isGrounded = true;
                    }
                }
            }
        }
        entity.Remove<IsColliderHit>();
    }
}
