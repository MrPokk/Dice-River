using System;
using BitterECS.Core;
using UINotDependence.Core;
using UnityEngine.UI;

public class UIHealthElement : UIPopup
{
    public Slider health;
    private EcsFilter _ecsEntities = new EcsFilter<EntitiesPresenter>()
        .Include<HealthComponent>()
        .Include<InputComponent>();

    private EcsEvent _ecsEvent;


    public override void Open()
    {
        _ecsEvent = new EcsEvent<EntitiesPresenter>()
        .SubscribeWhereEntity<IsHealthChanging>(e => e.Has<InputComponent>(), added: OnRefresh);

        foreach (var entity in _ecsEntities)
        {
            health.maxValue = entity.Get<HealthComponent>().maxHealth;
        }
        base.Open();
    }

    public override void Close()
    {
        _ecsEvent.Dispose();
        base.Close();
    }

    private void OnRefresh(EcsEntity entity)
    {
        ref var healthComp = ref entity.Get<HealthComponent>();
        health.value = healthComp.currentHealth;
    }


}
