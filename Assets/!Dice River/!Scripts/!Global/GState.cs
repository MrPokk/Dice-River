using System.Collections.Generic;
using BitterECS.Core;

public enum DifficultyTier
{
    Tier1_Base = 0,
    Tier1_Advanced = 1,
    Tier2_Base = 2,
    Tier2_Advanced = 3,
    Tier3_Base = 4
}

public class GState
{
    public bool isFirstStart;
    public DifficultyTier currentDifficulty;
    public float currentHazardChance;
    public int totalScrollDistance;
    public HashSet<UIProvider> collectedDiceTypes;

    public GState(DifficultyTier difficulty, float initialHazard)
    {
        isFirstStart = false;
        currentDifficulty = difficulty;
        currentHazardChance = initialHazard;
        collectedDiceTypes = new();
        totalScrollDistance = 0;
    }
}
