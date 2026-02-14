using UnityEngine;

[CreateAssetMenu(fileName = "ComplicationSettings", menuName = "Gameplay/Complication Settings")]
public class ComplicationSettings : ScriptableObject
{
    [Header("Complication Settings")]
    [Tooltip("How what distance to increase the speed")]
    public float distanceStep = 100f;

    [Tooltip("How much to increase the speed")]
    public float speedStep = 0.5f;

    public DifficultyTier difficultyStart;

    [Header("Limit speed")]
    public float minSpeed = 1f;
    public float maxSpeed = 10f;
}
