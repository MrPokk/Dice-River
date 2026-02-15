using UnityEngine;

[CreateAssetMenu(fileName = "ComplicationSettings", menuName = "Gameplay/Complication Settings")]
public class ComplicationSettings : ScriptableObject
{
    [Header("Complication Settings")]
    [Tooltip("Distance to increase speed and difficulty")]
    public float distanceStepToScroll = 100f;
    public float distanceStepToDifficulty = 100f;
    public int startGenerationToLine = 10;

    [Tooltip("How much to increase the speed")]
    public float speedStep = 0.5f;

    public DifficultyTier difficultyStart;

    [Header("Limit speed")]
    public float minSpeed = 1f;
    public float maxSpeed = 10f;
}
