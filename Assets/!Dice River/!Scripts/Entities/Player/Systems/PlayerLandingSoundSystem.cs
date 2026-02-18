using BitterECS.Core;

public class PlayerLandingSoundSystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _ecsEntitiesLanded = new EcsFilter<EntitiesPresenter>()
        .Include<GravityComponent>(c => c.isGrounded && c.verticalVelocity == 0);

    private EcsFilter _ecsEntitiesInAir = new EcsFilter<EntitiesPresenter>()
        .Include<GravityComponent>(c => !(c.isGrounded && c.verticalVelocity == 0));

    public void FixedRun()
    {
        foreach (var entity in _ecsEntitiesLanded)
        {
            entity.Add<IsGroundedSound>();
        }

        foreach (var entity in _ecsEntitiesInAir)
        {
            entity.Remove<IsGroundedSound>();
        }
    }
}
