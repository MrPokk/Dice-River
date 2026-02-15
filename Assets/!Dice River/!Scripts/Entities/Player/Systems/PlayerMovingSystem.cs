using BitterECS.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovingSystem : IEcsFixedRunSystem, IEcsInitSystem, IEcsRunSystem
{
    public Priority Priority => Priority.High;

    private const float SqrMagnitudeThreshold = 0.001f;
    private InputAction _moveAction;

    private EcsFilter _ecsFilter =
    Build.For<EntitiesPresenter>()
         .Filter()
         .WhereProvider<PlayerProvider>()
         .Include<InputComponent>()
         .Include<MovingComponent>()
         .Include<GravityComponent>()
         .Include<FacingComponent>();

    public void Init()
    {
        _moveAction = ControllableSystem.Inputs.Playable.Move;
    }
    public void Run()
    {
        foreach (var entity in _ecsFilter)
        {
            ref var input = ref entity.Get<InputComponent>();
            var raw = _moveAction?.ReadValue<Vector2>() ?? Vector2.zero;

            input.currentInput = raw.sqrMagnitude > 0.01f ? raw : Vector2.zero;
        }
    }

    public void FixedRun()
    {
        foreach (var entity in _ecsFilter)
        {
            var provider = entity.GetProvider<PlayerProvider>();
            ref var moving = ref entity.Get<MovingComponent>();
            var input = entity.Get<InputComponent>();
            var gravity = entity.Get<GravityComponent>();
            var facing = entity.Get<FacingComponent>();

            var cc = provider.characterController;
            Vector3 horizontalVelocity;

            var rawInput = input.currentInput;
            var moveDirection = new Vector3(rawInput.x, 0f, rawInput.y);

            if (gravity.isGrounded)
            {
                moveDirection = CheckToUpdateFacingDirection(entity, provider, moveDirection);
                horizontalVelocity = moveDirection * moving.velocity;
            }
            else
            {
                if (moveDirection.sqrMagnitude > SqrMagnitudeThreshold)
                {
                    moveDirection.Normalize();
                }
                horizontalVelocity = moveDirection * moving.jumpVelocityX;
            }

            cc.Move(horizontalVelocity * Time.fixedDeltaTime);

            entity.AddOrRemove<IsMovingComponent, Vector3>(new(), horizontalVelocity, i => i.sqrMagnitude > SqrMagnitudeThreshold);
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
        if (Mathf.Abs(directionX) < 0.01f) return;

        var scale = provider.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(directionX);
        provider.transform.localScale = scale;
    }
}
