using System.Collections.Generic;
using UnityEngine;

public class HandItem<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private List<T> _items = new();
    [SerializeField] private bool _rotateOrigin;

    [field: SerializeField]
    public int Max { get; private set; } = 4;

    public IReadOnlyCollection<T> CardReadOnlyCollection => _items;

    [Range(20, 50)]
    [SerializeField]
    private float _parabolaParameter = 20f;

    [SerializeField]
    private float _space = 1f;

    private void Update()
    {
        SetPose();
    }

    public int GetCountCard()
    {
        return _items.Count;
    }

    public void Add(T card)
    {
        if (!card || _items.Count >= Max)
            return;

        _items.Add(card);
        SetPose();
    }

    public void Remove(T card)
    {
        if (!card)
            return;

        _items.Remove(card);
        SetPose();
    }

    private void SetPose()
    {
        var cardCount = _items.Count;
        if (cardCount == 0) return;

        var centerOffset = (cardCount - 1) / 2.0f;

        for (var i = 0; i < cardCount; i++)
        {
            if (_items[i] == null) continue;

            var newPosition = ConvertOrigin(i, centerOffset);
            _items[i].transform.position = newPosition;
        }
    }

    private Vector3 ConvertOrigin(int elementIndex, float centerOffset)
    {
        var x = (elementIndex - centerOffset) * _space;
        var y = -(x * x) / _parabolaParameter;

        if (_rotateOrigin)
        {
            return new Vector3(y, x, 0);
        }
        else
        {
            return new Vector3(x, y, 0);
        }
    }
}
