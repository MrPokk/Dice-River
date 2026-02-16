using BitterECS.Core;

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
                moving.jumpVelocityX = 0f;
            }
        }
    }
}
