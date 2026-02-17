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

    public ComplicationGameplaySystem(RiverScrollingSystem riverScrolling, ComplicationSettings settings)
    {
        _riverScrolling = riverScrolling;
        _settings = settings;

        if (_settings == null) throw new("ComplicationSettings is null");

        float currentDist = _riverScrolling.TotalScrollDistance;

        _nextSpeedThreshold = CalculateNextThreshold(currentDist, _settings.distanceStepToScroll);
        _nextDifficultyThreshold = CalculateNextThreshold(currentDist, _settings.distanceStepToDifficulty);
    }

    public void Run()
    {
        if (_riverScrolling == null || _settings == null) return;

        var currentDist = _riverScrolling.TotalScrollDistance;

        if (currentDist >= _nextSpeedThreshold)
        {
            _riverScrolling.scrollSpeed = Mathf.Clamp(
                _riverScrolling.scrollSpeed + _settings.speedStep,
                _settings.minSpeed,
                _settings.maxSpeed
            );

            while (currentDist >= _nextSpeedThreshold)
            {
                _nextSpeedThreshold += _settings.distanceStepToScroll;
            }
        }

        if (currentDist >= _nextDifficultyThreshold)
        {
            IncreaseDifficulty();

            while (currentDist >= _nextDifficultyThreshold)
            {
                _nextDifficultyThreshold += _settings.distanceStepToDifficulty;
            }
        }
    }

    private static float CalculateNextThreshold(float currentDistance, float step)
    {
        if (step <= 0) return float.MaxValue;
        return (Mathf.Floor(currentDistance / step) + 1) * step;
    }

    private void IncreaseDifficulty()
    {
        var currentIdx = (int)StartupGameplay.GState.currentDifficulty;
        var maxIdx = (int)DifficultyTier.Tier3_Base;

        if (currentIdx < maxIdx)
        {
            StartupGameplay.GState.currentDifficulty = (DifficultyTier)(currentIdx + 1);
            Debug.Log($"Difficulty increased to: {StartupGameplay.GState.currentDifficulty}");
        }
    }
}
