using BitterECS.Core;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DiceBuoyancySystem : IEcsFixedRunSystem
{
    public Priority Priority => Priority.Low;

    private EcsFilter _diceFilter = new EcsFilter<DicePresenter>()
        .WhereProvider<DiceProvider>(p =>
            p.gameObject.scene.IsValid()
#if UNITY_EDITOR
            && !EditorUtility.IsPersistent(p)
#endif
        )
        .Include<WaveComponent>();

    public void FixedRun()
    {
        var time = Time.time;
        var deltaTime = Time.deltaTime;

        foreach (var diceEntity in _diceFilter)
        {
            var diceProvider = diceEntity.GetProvider<DiceProvider>();

#if UNITY_EDITOR
            if (EditorUtility.IsPersistent(diceProvider) ||
                UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
                continue;
#endif

            ref var waveComp = ref diceEntity.Get<WaveComponent>();

            if (diceProvider.spriteRoll == null || diceProvider.spriteSide == null) continue;

            if (!waveComp.initialized)
            {
                waveComp.initialRollLocalPos = diceProvider.spriteRoll.transform.localPosition;
                waveComp.initialSideLocalPos = diceProvider.spriteSide.transform.localPosition;
                waveComp.initialized = true;
            }

            var targetSink = waveComp.isDepressed ? 0.1f : 0f;
            var waveAmplitude = 0.04f;
            var waveIntensity = waveComp.isDepressed ? 0.3f : 1f;

            waveComp.currentSinkOffset = Mathf.Lerp(waveComp.currentSinkOffset, targetSink, deltaTime * 8f);

            var rootPos = diceProvider.transform.position;
            var wavePhase = rootPos.x * 0.5f + rootPos.z * 0.5f;

            var waveY = Mathf.Sin(time * 2.2f + wavePhase) * waveAmplitude * waveIntensity;
            var finalVisualY = waveY - waveComp.currentSinkOffset;

            diceProvider.spriteRoll.transform.localPosition = waveComp.initialRollLocalPos + new Vector3(0, finalVisualY, 0);
            diceProvider.spriteSide.transform.localPosition = waveComp.initialSideLocalPos + new Vector3(0, finalVisualY, 0);
        }
    }
}
