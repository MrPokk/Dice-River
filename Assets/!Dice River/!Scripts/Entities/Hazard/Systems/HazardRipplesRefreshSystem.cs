using BitterECS.Core;
using UnityEngine;

public class HazardRipplesRefreshSystem : IEcsRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _ecsEntities = new EcsFilter<EntitiesPresenter>()
        .WhereProvider<HazardProvider>(e => e.spriteRipple != null && e.spriteRipple.ripplesObject != null);

    public void Run()
    {
        var time = Time.time;

        foreach (var entity in _ecsEntities)
        {
            var environmentProvider = entity.GetProvider<HazardProvider>();

            var ripples = environmentProvider.spriteRipple.ripplesObject.transform;
            var dicePos = environmentProvider.transform.position;

            var wavePhase = dicePos.x * 0.6f + dicePos.z * 0.7f;

            var pulsation = Mathf.Sin(time * 5f + wavePhase);
            var currentScale = 1.1f + pulsation * 0.1f;

            ripples.localScale = Vector3.one * currentScale;
        }
    }
}
