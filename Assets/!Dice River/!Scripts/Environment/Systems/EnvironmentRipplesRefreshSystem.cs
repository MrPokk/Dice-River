using BitterECS.Core;
using UnityEngine;

public class EnvironmentRipplesRefreshSystem : IEcsRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _ecsEntities = new EcsFilter<EnvironmentPresenter>()
        .WhereProvider<EnvironmentProvider>(e => e.spriteRipple != null && e.spriteRipple.ripplesObject != null);

    public void Run()
    {
        var time = Time.time;

        foreach (var entity in _ecsEntities)
        {
            var environmentProvider = entity.GetProvider<EnvironmentProvider>();
            var ripplesTransform = environmentProvider.spriteRipple.ripplesObject.transform;

            var rootPos = environmentProvider.transform.position;

            var wavePhase = rootPos.x * 0.6f + rootPos.z * 0.7f;
            var pulsation = Mathf.Sin(time * 5f + wavePhase);
            var currentScale = 1.1f + pulsation * 0.1f;

            var baseScale = new Vector3(0.5f, 0.5f, 1f);
            ripplesTransform.localScale = baseScale * currentScale;

        }
    }
}
