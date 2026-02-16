using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceGroupVAR", menuName = "Dice System/Dice Group")]
public class DiceGroupSO : ScriptableObject
{
    [Tooltip("Level at which this group of dice will be available")]
    public DifficultyTier level;

    public List<DiceProvider> dice;
}
