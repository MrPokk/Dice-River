using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HandLoadStackPrefab", menuName = "Settings/HandLoadStackPrefab")]
public class HandLoadStackPrefab : ScriptableObject
{
    [SerializeField] private List<DiceProvider> _diceProviders;
    public IReadOnlyCollection<DiceProvider> DiceProviders => _diceProviders;
}
