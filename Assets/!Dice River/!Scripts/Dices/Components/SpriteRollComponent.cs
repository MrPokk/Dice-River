using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteRollComponent : MonoBehaviour
{
    [SerializeField] private Color _topRollColor;
    [SerializeField] private SpriteRenderer _topRollSprite;

    [SerializeField] private SpriteRollPrefab[] _rollPrefab;
    private SpriteRollPrefab _currentRoll;

    private Dictionary<SpriteRollPrefab, SpriteRenderer[]> _spriteRenderers = new();

    private void Awake()
    {
        foreach (var roll in _rollPrefab)
        {
            _spriteRenderers.Add(roll, roll.prefab.GetComponentsInChildren<SpriteRenderer>());
        }
    }

    private void OnValidate()
    {
        SetColorTopRoll();
    }

    private void SetColorTopRoll()
    {
        if (_topRollSprite == null) return;
        _topRollSprite.color = _topRollColor;
    }

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

    public void SetAlpha(float alpha)
    {
        if (_currentRoll == null || _currentRoll.prefab == null) return;

        SetAlphaValue(alpha, _topRollSprite);
        var renderers = _spriteRenderers[_currentRoll];
        foreach (var source in renderers)
        {
            SetAlphaValue(alpha, source);
        }
    }

    private static void SetAlphaValue(float alpha, SpriteRenderer source)
    {
        var color = source.color;
        color.a = alpha;
        source.color = color;
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
