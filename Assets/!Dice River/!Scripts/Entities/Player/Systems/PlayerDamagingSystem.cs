using System;
using BitterECS.Core;
using Unity.Mathematics;
using UnityEngine;

public class PlayerDamagingSystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.High;

    private EcsFilter _ecsEntities = new EcsFilter<EntitiesPresenter>()
        .Include<HealthComponent>()
        .Include<DamageToIntervalComponent>()
        .Include<GravityComponent>(c => c.isGrounded && c.verticalVelocity == 0);

    public void FixedRun()
    {
        var deltaTime = Time.fixedDeltaTime;

        foreach (var entity in _ecsEntities)
        {
            ref var health = ref entity.Get<HealthComponent>();
            var damageComp = entity.Get<DamageToIntervalComponent>();

            health.timeImmunity -= deltaTime;

            if (health.timeImmunity <= 0)
            {
                var newHealth = health.currentHealth - damageComp.damage;
                health.SetHealth(newHealth);
                entity.AddFrameToEvent<IsHealthChanging>();

                health.lastDamage = damageComp.damage;
                health.timeImmunity = damageComp.damageIntervalSecond;
            }
        }
    }
}
