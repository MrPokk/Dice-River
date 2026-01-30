using System;
using BitterECS.Core;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovingSystem : IEcsFixedRunSystem, IEcsInitSystem
{
    public Priority Priority => Priority.Medium;

    private EcsFilter _ecsFilter =
    Build.For<EntitiesPresenter>()
         .Filter()
         .WhereProvider<EntitiesProvider>()
         .Include<InputComponent>()
         .Include<MovingComponent>();

    public void Init()
    {
        var move = ControllableSystem.Inputs.Playable.Move;
        ControllableSystem.SubscribePerformed(move, MovePressingSystem);
        ControllableSystem.SubscribeCanceled(move, MoveUnPressingSystem);
    }

    private void MovePressingSystem(CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        foreach (var entity in _ecsFilter)
        {
            entity.Get<InputComponent>().currentInput = direction;
        }
    }

    private void MoveUnPressingSystem(CallbackContext context)
    {
        foreach (var entity in _ecsFilter)
        {
            entity.Get<InputComponent>().currentInput = Vector2.zero;
        }
    }

    public void FixedRun()
    {
        foreach (var entity in _ecsFilter)
        {
            var provider = entity.GetProvider<EntitiesProvider>();
            ref var moving = ref entity.Get<MovingComponent>();
            ref var input = ref entity.Get<InputComponent>();

            var rb = provider.rigidbody;
            var moveDirection = input.currentInput;

            if (moveDirection.sqrMagnitude > 0.001f)
            {
                if (moveDirection.sqrMagnitude > 1f) moveDirection.Normalize();

                entity.GetOrAdd<FacingComponent>().direction = moveDirection;
                FlipSprite(provider, moveDirection);
            }

            rb.linearVelocity = moveDirection * moving.speed;
            entity.AddOrRemove<IsMovingComponent, Vector2>(new(), moveDirection, i => i != Vector2.zero);
        }
    }

    private static void FlipSprite(EntitiesProvider provider, Vector2 moveDirection)
    {
        if (Mathf.Abs(moveDirection.x) < 0.01f) return;
        var scale = provider.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveDirection.x);
        provider.transform.localScale = scale;
    }
}
