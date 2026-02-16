using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceContainerVAR", menuName = "Dice System/Dice Container")]
public class DiceContainerSO : ScriptableObject
{
    public List<DiceGroupSO> groups;

    [Header("Calculated Data (Read Only)")]
    [SerializeField, ReadOnly] private DifficultyTier _averageDifficultyDisplay;

    public DifficultyTier AverageDifficulty
    {
        get
        {
            if (groups == null || groups.Count == 0)
                return DifficultyTier.Tier1_Base;

            float totalDifficulty = 0;
            var validGroupsCount = 0;

            foreach (var group in groups)
            {
                if (group != null)
                {
                    totalDifficulty += (int)group.level;
                    validGroupsCount++;
                }
            }

            if (validGroupsCount == 0)
                return DifficultyTier.Tier1_Base;

            var averageValue = Mathf.RoundToInt(totalDifficulty / validGroupsCount);
            var maxTier = (int)DifficultyTier.Tier3_Base;
            averageValue = Mathf.Clamp(averageValue, 0, maxTier);

            return (DifficultyTier)averageValue;
        }
    }

    private void OnValidate()
    {
        _averageDifficultyDisplay = AverageDifficulty;
    }

    [ContextMenu("Refresh Average Difficulty")]
    private void RefreshDifficulty()
    {
        OnValidate();
        Debug.Log($"Average Difficulty for {name}: {AverageDifficulty}");
    }
}
