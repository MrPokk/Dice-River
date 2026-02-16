using BitterECS.Core;
using UnityEngine;

public class JumpApplySystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Medium;

    private EcsEvent _jumpFilter =
    new EcsEvent<EntitiesPresenter>()
        .SubscribeWhereEntity<JumpEvent>(
            e => EcsConditions.Has<
            JumpComponent,
            GravityComponent,
            MovingComponent,
            FacingComponent,
            InputComponent>(e),
            added: OnJumpingEvent
        );

    private static void OnJumpingEvent(EcsEntity entity)
    {
        ref var gravity = ref entity.Get<GravityComponent>();
        if (!gravity.isGrounded) return;

        var jumpData = entity.Get<JumpComponent>();
        ref var moving = ref entity.Get<MovingComponent>();
        var input = entity.Get<InputComponent>();

        var vy = Mathf.Sqrt(2f * gravity.gravity * jumpData.jumpHeight);
        var vx = 0f;

        if (input.currentInput.sqrMagnitude > 0.001f)
        {
            var timeInAir = 2f * vy / gravity.gravity;
            vx = jumpData.jumpLength / timeInAir;
        }

        gravity.verticalVelocity = vy;
        moving.jumpVelocityX = vx;
    }
}
