using System;
using BitterECS.Core;
using UnityEngine;

public struct ComplicationGameplaySystem : IEcsRunSystem
{
    public readonly Priority Priority => Priority.High;

    private readonly RiverScrollingSystem _riverScrolling;
    private readonly ComplicationSettings _settings;

    private float _nextSpeedThreshold;
    private float _nextDifficultyThreshold;
    private float _nextHazardThreshold;

    public ComplicationGameplaySystem(RiverScrollingSystem riverScrolling, ComplicationSettings settings)
    {
        _riverScrolling = riverScrolling ?? throw new ArgumentNullException(nameof(riverScrolling));
        _settings = settings ?? throw new ArgumentNullException(nameof(settings));

        var totalDistance = _riverScrolling.TotalScrollDistance;
        _nextSpeedThreshold = CalcNext(totalDistance, _settings.distanceStepToScroll);
        _nextDifficultyThreshold = CalcNext(totalDistance, _settings.distanceStepToDifficulty);
        _nextHazardThreshold = CalcNext(totalDistance, _settings.distanceStepToHazard);
    }

    public void Run()
    {
        var riverSetting = _riverScrolling;
        var totalDistance = riverSetting.TotalScrollDistance;
        var complicationSettings = _settings;

        GFlow.GState.totalScrollDistance = (int)riverSetting.TotalScrollDistance;

        TryUpdate(totalDistance, ref _nextSpeedThreshold, complicationSettings.distanceStepToScroll, () =>
            IncreaseScrollSpeed(riverSetting, complicationSettings));

        TryUpdate(totalDistance, ref _nextDifficultyThreshold, complicationSettings.distanceStepToDifficulty, () =>
            IncreaseDifficulty());

        TryUpdate(totalDistance, ref _nextHazardThreshold, complicationSettings.distanceStepToHazard, () =>
            IncreaseHazardChance(complicationSettings));
    }

    private static void IncreaseScrollSpeed(RiverScrollingSystem riverSetting, ComplicationSettings complicationSettings)
    {
        riverSetting.scrollSpeed = Mathf.Clamp(
            riverSetting.scrollSpeed + complicationSettings.speedStep,
            complicationSettings.minSpeed,
            complicationSettings.maxSpeed);
        Debug.Log($"[Complication] Speed updated: {riverSetting.scrollSpeed}");
    }

    private static void IncreaseHazardChance(ComplicationSettings complicationSettings)
    {
        GFlow.GState.currentHazardChance = Mathf.Clamp(
            GFlow.GState.currentHazardChance + complicationSettings.hazardStep,
            0,
            complicationSettings.maxHazardChance);
        Debug.Log($"[Complication] Hazard chance updated: {GFlow.GState.currentHazardChance:F2}");
    }

    private void TryUpdate(float currentDist, ref float threshold, float step, Action action)
    {
        if (step <= 0 || currentDist < threshold) return;

        action();
        while (currentDist >= threshold) threshold += step;
    }

    private static float CalcNext(float dist, float step) =>
        step <= 0 ? float.MaxValue : (Mathf.Floor(dist / step) + 1) * step;

    private static void IncreaseDifficulty()
    {
        if (GFlow.GState.currentDifficulty < DifficultyTier.Tier3_Base)
        {
            GFlow.GState.currentDifficulty++;
            Debug.Log($"[Complication] Difficulty increased to: {GFlow.GState.currentDifficulty}");
        }
    }
}
