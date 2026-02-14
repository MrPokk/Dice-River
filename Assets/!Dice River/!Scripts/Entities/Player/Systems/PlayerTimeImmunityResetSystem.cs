using BitterECS.Core;

public class PlayerTimeImmunityResetSystem : IEcsInitSystem
{
    public Priority Priority => Priority.High;

    private EcsEvent _ecsEvent = new EcsEvent<EntitiesPresenter>()
        .SubscribeWhereEntity<IsOnLifting>(e => EcsConditions.Has<
        HealthComponent,
        DamageToIntervalComponent>(e),
        added: OnReset);

    private EcsFilter _ecsEntities = new EcsFilter<EntitiesPresenter>()
    .Include<HealthComponent>()
    .Include<DamageToIntervalComponent>();

    public void Init()
    {
        foreach (var entity in _ecsEntities)
        {
            entity.Get<HealthComponent>().timeImmunity = 0;
        }
    }

    private static void OnReset(EcsEntity entity)
    {
        ref var healthComp = ref entity.Get<HealthComponent>();

        healthComp.timeImmunity = 0;
    }

}
