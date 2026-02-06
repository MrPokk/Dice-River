using System;
using System.Linq;
using UnityEngine;

public class SpriteRollComponent : MonoBehaviour
{
    [SerializeField] private SpriteRollPrefab[] _rollPrefab;
    private SpriteRollPrefab _currentRoll;

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
    One = 0,
    Two = 1,
    Three = 2,
    Four = 3,
    Five = 4,
    Six = 5
}
