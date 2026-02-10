using BitterECS.Core;
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
            var gravityComponent = entity.Get<GravityComponent>();
            if (gravityComponent.verticalVelocity != 0)
                entity.AddFrameToEvent(new JumpEvent());
        }
    }
}
