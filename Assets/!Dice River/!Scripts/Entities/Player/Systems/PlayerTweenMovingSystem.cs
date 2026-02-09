using System;
using BitterECS.Core;
using DG.Tweening;
using UnityEngine;
using static BitterECS.Core.EcsFilter;

public class PlayerTweenMovingSystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.Medium;

    private const float Angle = 7f;
    private const float Duration = 0.2f;
    private const float ReferenceSpeed = 5f;

    private Filter MovingEntities =>
        new EcsFilter<EntitiesPresenter>()
            .Include<IsMovingComponent>()
            .Include<MovingComponent>()
            .WhereProvider<EntitiesProvider>()
            .Entities();

    private readonly EcsEvent ecsEvent =
        new EcsEvent<EntitiesPresenter>()
        .SubscribeWhereEntity<IsMovingComponent>(e => e.HasProvider<EntitiesProvider>(), removed: OnRemoved);

    private static void OnRemoved(EcsEntity entity)
    {
        var provider = entity.GetProvider<EntitiesProvider>();
        if (provider == null) return;

        var transform = provider.transform;
        transform.DOKill();
        transform.DOLocalRotate(Vector3.zero, 0.1f);
    }

    public void FixedRun()
    {
        foreach (var entity in MovingEntities)
        {
            var transform = entity.GetProvider<EntitiesProvider>().transform;
            var speed = entity.Get<MovingComponent>().velocity;
            var speedMultiplier = speed / ReferenceSpeed;

            var tweens = DOTween.TweensByTarget(transform);

            if (tweens == null || tweens.Count == 0)
            {
                transform.DOLocalRotate(new Vector3(0, 0, Angle), Duration)
                    .SetEase(Ease.InOutQuad)
                    .SetLoops(-1, LoopType.Yoyo)
                    .Play();
            }
            else
            {
                for (var i = 0; i < tweens.Count; i++)
                {
                    tweens[i].timeScale = speedMultiplier > 0 ? speedMultiplier : 0.1f;
                }
            }
        }
    }
}
