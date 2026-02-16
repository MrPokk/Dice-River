using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public class RipplesRefreshSystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _ecsEntities = new EcsFilter<EntitiesPresenter>()
        .Include<SpriteRipplesComponent>(c => c.ripplesObject != null);

    public void FixedRun()
    {
        var time = Time.time;

        foreach (var entity in _ecsEntities)
        {
            var environmentProvider = entity.GetProvider<ProviderEcs>();
            ref var spriteRipplesComponent = ref entity.Get<SpriteRipplesComponent>();

            var rootPos = environmentProvider.transform.position;

            var wavePhase = rootPos.x * 0.6f + rootPos.z * 0.7f;
            var pulsation = Mathf.Sin(time * 5f + wavePhase);

            var pulsationMultiplier = 1.1f + pulsation * 0.1f;

            var baseScale = spriteRipplesComponent.baseScale;

            var ripplesTransform = spriteRipplesComponent.ripplesObject.transform;
            ripplesTransform.localScale = baseScale * pulsationMultiplier;
        }
    }
}
