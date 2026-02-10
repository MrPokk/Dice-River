using BitterECS.Core;
using UnityEngine;

public class PlayerDamagingSystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.High;

    private EcsFilter _ecsEvent = new EcsFilter<EntitiesPresenter>()
        .Include<HealthComponent>()
        .Include<DamageComponent>()
        .Include<GravityComponent>(c => c.isGrounded && c.verticalVelocity == 0);

    public void FixedRun()
    {
        var deltaTime = Time.fixedDeltaTime;

        foreach (var entity in _ecsEvent)
        {
            ref var health = ref entity.Get<HealthComponent>();
            var damageComp = entity.Get<DamageComponent>();

            health.timeImmunity -= deltaTime;

            if (health.timeImmunity <= 0)
            {
                var newHealth = health.currentHealth - damageComp.damage;
                health.SetHealth(newHealth);
                entity.AddFrameToEvent<IsHealthChanging>(new());

                health.lastDamage = damageComp.damage;
                health.timeImmunity = damageComp.damageIntervalSecond;
            }
        }
    }
}


public struct IsHealthChanging
{

}
