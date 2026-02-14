using BitterECS.Core;
using UnityEngine;

public class HazardWavingRefreshSystem : IEcsRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _ecsEntities = new EcsFilter<EntitiesPresenter>()
        .WhereProvider<HazardProvider>();

    public void Run()
    {
        var time = Time.time;

        foreach (var entity in _ecsEntities)
        {
            var hazardProvider = entity.GetProvider<HazardProvider>();
            var hazardTransform = hazardProvider.transform;

            var pos = hazardTransform.position;

            var wavePhase = pos.x * 0.6f + pos.z * 0.7f;
            var bobbingY = Mathf.Sin(time * 2.5f + wavePhase) * 0.12f;

            hazardTransform.position = new Vector3(pos.x, bobbingY, pos.z);
        }
    }
}
