using BitterECS.Core;

public class PlayerAnimationSystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.Medium;

    private EcsFilter _ecsMovingEntities =
    Build.For<EntitiesPresenter>()
    .Filter()
    .WhereProvider<PlayerProvider>()
    .Include<IsMovingComponent>();
    private EcsFilter _ecsNotMovingEntities =
    Build.For<EntitiesPresenter>()
    .Filter()
    .WhereProvider<PlayerProvider>()
    .Exclude<IsMovingComponent>();

    public void FixedRun()
    {
        //foreach (var entity in _ecsMovingEntities)
        //{
        //    entity.GetProvider<PlayerProvider>().animator.SetBool("IsRun", true);
        //}

        //foreach (var entity in _ecsNotMovingEntities)
        //{
        //    entity.GetProvider<PlayerProvider>().animator.SetBool("IsRun", false);
        //}
    }
}
