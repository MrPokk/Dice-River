using System;
using System.Collections.Generic;
using System.Linq; // Обязательно добавляем для LINQ
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct DiceContainer
{
    [Range(0, 1f)] public float tierUnderChance;
    [Range(0, 1f)] public float tierOverChance;
    public List<DiceContainerSO> settings;
#if UNITY_EDITOR
    [Header("Debug View (Auto-Calculated)")]
    [SerializeField, ReadOnly] private List<DiceProvider> _allDiceTypes;

    public void RefreshDiceList()
    {
        if (settings == null)
        {
            _allDiceTypes = new List<DiceProvider>();
            return;
        }

        _allDiceTypes = settings
            .Where(container => container != null && container.groups != null)
            .SelectMany(container => container.groups)
            .Where(group => group != null && group.dice != null)
            .SelectMany(group => group.dice)
            .Where(dice => dice != null)
            .Distinct()
            .ToList();
    }
#endif
}

public class DiceContainerProvider : ProviderEcs<DiceContainer>
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        var tempContainer = _value;
        tempContainer.RefreshDiceList();
        _value = tempContainer;
    }
#endif
}
