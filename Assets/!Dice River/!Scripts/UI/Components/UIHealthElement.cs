using System;
using BitterECS.Core;
using DG.Tweening;
using UINotDependence.Core;
using UnityEngine;
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
            var healthComp = entity.Get<HealthComponent>();
            health.maxValue = healthComp.maxHealth;
            health.value = healthComp.currentHealth;
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
        var defaultScale = transform.localScale;
        transform.localScale = transform.localScale / 2;
        transform.DOScale(defaultScale, 0.5f).SetEase(Ease.OutBack).Play();
    }
}
