using System;
using BitterECS.Core;
using UnityEngine;

public struct ComplicationGameplaySystem : IEcsRunSystem
{
    public readonly Priority Priority => Priority.High;

    private readonly RiverScrollingSystem _riverScrolling;
    private readonly ComplicationSettings _settings;
    private float _nextThreshold;

    public ComplicationGameplaySystem(RiverScrollingSystem riverScrolling, ComplicationSettings settings)
    {
        _riverScrolling = riverScrolling;
        _settings = settings;

        var step = _settings != null ? _settings.distanceStepToDifficulty : throw new("ComplicationSettings is null");
        _nextThreshold = (Mathf.Floor(_riverScrolling.TotalScrollDistance / step) + 1) * step;
    }

    public void Run()
    {
        if (_riverScrolling == null || _settings == null) return;

        if (_riverScrolling.TotalScrollDistance >= _nextThreshold)
        {
            _riverScrolling.scrollSpeed = Mathf.Clamp(_riverScrolling.scrollSpeed + _settings.speedStep, _settings.minSpeed, _settings.maxSpeed);

            IncreaseDifficulty();

            while (_riverScrolling.TotalScrollDistance >= _nextThreshold)
            {
                _nextThreshold += _settings.distanceStepToDifficulty;
            }
        }
    }

    private void IncreaseDifficulty()
    {
        var currentIdx = (int)StartupGameplay.GState.currentDifficulty;
        var maxIdx = (int)DifficultyTier.Tier3_Base;

        if (currentIdx < maxIdx)
        {
            StartupGameplay.GState.currentDifficulty = (DifficultyTier)(currentIdx + 1);
        }
        Debug.Log($"Difficulty increased to: {StartupGameplay.GState.currentDifficulty}");
    }
}
