using System;
using BitterECS.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJumpingSystem : IEcsInitSystem
{
    public Priority Priority => Priority.High;

    private EcsFilter _ecsEntities = new EcsFilter<EntitiesPresenter>()
        .Include<JumpComponent>()
        .Include<InputComponent>();

    public void Init()
    {
        var jumpAction = ControllableSystem.Inputs.Playable.Jumping;
        ControllableSystem.SubscribePerformed(jumpAction, OnJump);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        foreach (var entity in _ecsEntities)
        {
            entity.AddFrameToEvent(new JumpEvent());
        }
    }
}

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
            FacingComponent>(e),
            added: OnJumpingEvent
        );

    private static void OnJumpingEvent(EcsEntity entity)
    {
        ref var gravity = ref entity.Get<GravityComponent>();
        if (!gravity.isGrounded) return;

        var jumpData = entity.Get<JumpComponent>();
        ref var moving = ref entity.Get<MovingComponent>();

        var vy = Mathf.Sqrt(2f * gravity.gravity * jumpData.jumpHeight);
        var timeInAir = 2f * vy / gravity.gravity;
        var vx = jumpData.jumpLength / timeInAir;

        gravity.verticalVelocity = vy;
        moving.speed = (int)vx;
    }
}

public class JumpResetSystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _groundedFilter = new EcsFilter<EntitiesPresenter>()
        .Include<MovingComponent>()
        .Include<GravityComponent>();

    public void FixedRun()
    {
        foreach (var entity in _groundedFilter)
        {
            ref var gravityComp = ref entity.Get<GravityComponent>();

            if (gravityComp.isGrounded && gravityComp.verticalVelocity <= 0)
            {
                ref var moving = ref entity.Get<MovingComponent>();
                if (moving.speed != moving.baseSpeed)
                {
                    moving.speed = moving.baseSpeed;
                }
            }
        }
    }
}
