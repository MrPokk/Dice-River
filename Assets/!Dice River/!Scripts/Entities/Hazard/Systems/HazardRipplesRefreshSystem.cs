using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public class HazardRipplesRefreshSystem : IEcsRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _ecsEntities = new EcsFilter<EntitiesPresenter>()
        .Include<SpriteRipplesComponent>(c => c.ripplesObject != null);

    public void Run()
    {
        var time = Time.time;

        foreach (var entity in _ecsEntities)
        {
            var environmentProvider = entity.GetProvider<ProviderEcs>();
            var spriteRipplesComponent = entity.Get<SpriteRipplesComponent>();

            var ripples = spriteRipplesComponent.ripplesObject.transform;
            var dicePos = environmentProvider.transform.position;

            var wavePhase = dicePos.x * 0.6f + dicePos.z * 0.7f;

            var pulsation = Mathf.Sin(time * 5f + wavePhase);
            var currentScale = 1.1f + pulsation * 0.1f;

            ripples.localScale = Vector3.one * currentScale;
        }
    }
}
