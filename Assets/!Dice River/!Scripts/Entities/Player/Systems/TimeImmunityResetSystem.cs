using BitterECS.Core;

public class TimeImmunityResetSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private EcsEvent _ecsEvent = new EcsEvent<EntitiesPresenter>()
        .SubscribeWhereEntity<IsOnLifting>(e => EcsConditions.Has<
        HealthComponent,
        DamageToIntervalComponent>(e),
        added: OnReset);

    private static void OnReset(EcsEntity entity)
    {
        ref var healthComp = ref entity.Get<HealthComponent>();
        var damageComp = entity.Get<DamageToIntervalComponent>();

        healthComp.timeImmunity = damageComp.damageIntervalSecond;
    }
}
