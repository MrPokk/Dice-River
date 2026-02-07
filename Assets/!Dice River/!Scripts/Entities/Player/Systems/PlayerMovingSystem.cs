using System;
using BitterECS.Core;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovingSystem : IEcsFixedRunSystem, IEcsInitSystem
{
    public Priority Priority => Priority.High;

    private const float SqrMagnitudeThreshold = 0.001f;
    private EcsFilter _ecsFilter =
    Build.For<EntitiesPresenter>()
         .Filter()
         .WhereProvider<PlayerProvider>()
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
            var provider = entity.GetProvider<PlayerProvider>();
            ref var moving = ref entity.Get<MovingComponent>();
            ref var input = ref entity.Get<InputComponent>();

            var cc = provider.characterController;
            var rawInput = input.currentInput;
            var moveDirection = new Vector3(rawInput.x, 0f, rawInput.y);

            moveDirection = CheckToUpdateFacingDirection(entity, provider, moveDirection);

            var velocity = moveDirection * moving.speed;

            cc.Move(velocity * Time.fixedDeltaTime);
            entity.AddOrRemove<IsMovingComponent, Vector3>(new(), moveDirection, i => i != Vector3.zero);
        }
    }

    private static Vector3 CheckToUpdateFacingDirection(EcsEntity entity, PlayerProvider provider, Vector3 moveDirection)
    {
        if (moveDirection.sqrMagnitude <= SqrMagnitudeThreshold)
        {
            return moveDirection;
        }

        if (moveDirection.sqrMagnitude > 1f)
        {
            moveDirection.Normalize();
        }

        entity.GetOrAdd<FacingComponent>().direction = moveDirection;
        FlipSprite(provider, moveDirection.x);

        return moveDirection;
    }

    private static void FlipSprite(EntitiesProvider provider, float directionX)
    {
        if (Mathf.Abs(directionX) < 0.01f)
        {
            return;
        }

        var scale = provider.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(directionX);
        provider.transform.localScale = scale;
    }
}
