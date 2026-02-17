using BitterECS.Core;
using UnityEngine;

public class PickupWavingRefreshSystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _ecsEntities = new EcsFilter<EntitiesPresenter>()
        .WhereProvider<PickupProvider>()
        .Include<WaveComponent>();

    public void FixedRun()
    {
        var time = Time.time;

        foreach (var entity in _ecsEntities)
        {
            var pickupProvider = entity.GetProvider<PickupProvider>();
            var hazardTransform = pickupProvider.transform;

            var pos = hazardTransform.position;

            var wavePhase = pos.x * 0.6f + pos.z * 0.7f;
            var bobbingY = Mathf.Sin(time * 2.5f + wavePhase) * 0.2f;

            hazardTransform.position = new Vector3(pos.x, bobbingY, pos.z);
        }
    }
}
