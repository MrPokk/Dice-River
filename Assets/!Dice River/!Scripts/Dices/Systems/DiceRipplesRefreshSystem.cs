using BitterECS.Core;
using UnityEngine;

public class DiceRipplesRefreshSystem : IEcsRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _ecsEntities = new EcsFilter<DicePresenter>()
        .WhereProvider<DiceProvider>(e => e.spriteSide != null && e.spriteSide.ripplesObject != null);

    public void Run()
    {
        var time = Time.time;

        foreach (var entity in _ecsEntities)
        {
            var diceProvider = entity.GetProvider<DiceProvider>();

            var ripples = diceProvider.spriteSide.ripplesObject.transform;
            var dicePos = diceProvider.transform.position;

            var wavePhase = dicePos.x * 0.6f + dicePos.z * 0.7f;

            var pulsation = Mathf.Sin(time * 5f + wavePhase);
            var currentScale = 1.1f + pulsation * 0.1f;

            ripples.localScale = Vector3.one * currentScale;
        }
    }
}
