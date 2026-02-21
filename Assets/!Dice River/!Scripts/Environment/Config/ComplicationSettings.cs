using UnityEngine;

[CreateAssetMenu(fileName = "ComplicationSettings", menuName = "Gameplay/Complication Settings")]
public class ComplicationSettings : ScriptableObject
{
    [Header("Complication Settings")]
    public RiverSettings settings;
    public RiverHazardSettings HazardSettings => settings.hazardSettings;

    [Header("Hazard Progression (Setting)")]
    public float distanceStepToHazard = 50f;
    public float hazardStep = 0.02f;
    public float maxHazardChance = 0.5f;

    [Header("River Scroll Progression (Setting)")]
    public int startGenerationToLine = 10;
    [Tooltip("Distance to increase speed and difficulty")]
    public float distanceStepToScroll = 100f;
    [Tooltip("How much to increase the speed")]
    public float speedStep = 0.5f;
    public float minSpeed = 1f;
    public float maxSpeed = 10f;

    [Header("Difficulty Progression (Setting)")]
    public DifficultyTier difficultyStart;
    public float distanceStepToDifficulty = 100f;

}
