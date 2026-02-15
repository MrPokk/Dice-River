using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public class EnvironmentRipplesRefreshSystem : IEcsRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _ecsEntities = new EcsFilter<EnvironmentPresenter>()
         .Include<SpriteRipplesComponent>(c => c.ripplesObject != null);

    public void Run()
    {
        var time = Time.time;

        foreach (var entity in _ecsEntities)
        {
            var environmentProvider = entity.GetProvider<ProviderEcs>();
            var spriteRipplesComponent = entity.Get<SpriteRipplesComponent>();

            var rootPos = environmentProvider.transform.position;

            var wavePhase = rootPos.x * 0.6f + rootPos.z * 0.7f;
            var pulsation = Mathf.Sin(time * 5f + wavePhase);
            var currentScale = 1.1f + pulsation * 0.1f;

            var baseScale = new Vector3(0.5f, 0.5f, 1f);
            spriteRipplesComponent.ripplesObject.transform.localScale = baseScale * currentScale;
        }
    }
}
