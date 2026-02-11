using UnityEngine;
using BitterECS.Core;

public class PlayerLiftingSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private EcsEvent _ecsEvent =
        new EcsEvent<EntitiesPresenter>()
        .SubscribeWhereEntity<IsColliderHit>(e =>
        e.Has<GravityComponent>(c => c.verticalVelocity == 0),
        added: OnLifting);

    private static void OnLifting(EcsEntity entity)
    {
        ref var hitData = ref entity.Get<IsColliderHit>().hit;
        if (!hitData.collider.gameObject.TryGetComponent<DiceProvider>(out _))
        {
            return;
        }
        var provider = entity.GetProvider<EntitiesProvider>();
        var transform = provider.transform;

        if (!transform.TryGetComponent<CharacterController>(out var controller))
        {
            return;
        }

        var diceCollider = hitData.collider;
        var diceTopY = diceCollider.bounds.max.y;

        var footOffset = controller.height / 2f;
        var targetY = diceTopY + footOffset + controller.skinWidth;

        if (targetY > transform.position.y)
        {
            var diceCenter = diceCollider.bounds.center;
            var newPosition = new Vector3(diceCenter.x, targetY, diceCenter.z);

            controller.enabled = false;
            transform.position = newPosition;
            controller.enabled = true;

            entity.AddFrameToEvent<IsOnLifting>();
        }
    }
}
