using System;
using BitterECS.Core;
using UnityEngine;

public class ComplicationGameplaySystem : IEcsRunSystem
{
    public Priority Priority => Priority.High;

    private readonly RiverScrolling _riverScrolling;
    private readonly ComplicationSettings _settings;

    private float _nextThreshold;

    public ComplicationGameplaySystem() { }

    public ComplicationGameplaySystem(RiverScrolling riverScrolling, ComplicationSettings settings)
    {
        _riverScrolling = riverScrolling;
        _settings = settings;

        var step = _settings != null ? _settings.distanceStep : 100f;
        _nextThreshold = (Mathf.Floor(_riverScrolling.TotalOffsetZ / step) + 1) * step;
    }

    public void Run()
    {
        if (_riverScrolling == null || _settings == null) return;

        if (_riverScrolling.TotalOffsetZ >= _nextThreshold)
        {
            var targetSpeed = _riverScrolling.scrollSpeed + _settings.speedStep;

            _riverScrolling.scrollSpeed = Mathf.Clamp(targetSpeed, _settings.minSpeed, _settings.maxSpeed);
            while (_riverScrolling.TotalOffsetZ >= _nextThreshold)
            {
                _nextThreshold += _settings.distanceStep;
            }

            if (_riverScrolling.scrollSpeed >= _settings.maxSpeed)
            {
                Debug.Log($"[Complication] Max speed reached: {_riverScrolling.scrollSpeed}. Next threshold: {_nextThreshold}m");
            }
            else
            {
                Debug.Log($"[Complication] Speed increased: {_riverScrolling.scrollSpeed}. Next boost at: {_nextThreshold}m");
            }
        }
    }
}
