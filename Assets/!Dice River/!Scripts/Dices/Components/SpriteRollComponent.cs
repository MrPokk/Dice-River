using System;
using System.Linq;
using UnityEngine;

public class SpriteRollComponent : MonoBehaviour
{
    [SerializeField] private SpriteRollPrefab[] _rollPrefab;
    private SpriteRollPrefab _currentRoll;

    public void Select(int value)
    {
        if (value < 1 || value > 6)
        {
            throw new Exception("Invalid value");
        }

        Select((RollIndex)value);
    }

    public void Select(RollIndex value)
    {
        _currentRoll = _rollPrefab.FirstOrDefault(x => x.index == value) ?? throw new Exception("Not found roll prefab");
        DisableAllDots();
        _currentRoll.prefab.SetActive(true);
    }

    public SpriteRollPrefab GetCurrentRollPrefab()
    {
        return _currentRoll;
    }

    public void DisableAllDots()
    {
        foreach (var rollPrefabData in _rollPrefab)
        {
            if (rollPrefabData.prefab == null)
            {
                continue;
            }

            rollPrefabData.prefab.SetActive(false);
        }
    }
}

[Serializable]
public class SpriteRollPrefab
{
    public GameObject prefab;
    public RollIndex index;
}

public enum RollIndex
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6
}
