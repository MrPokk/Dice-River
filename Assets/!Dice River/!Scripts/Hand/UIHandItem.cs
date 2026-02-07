using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIHandItem<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private List<T> _items = new();
    [SerializeField] private bool _isVerticalLayout;

    [field: SerializeField]
    public int Max { get; private set; } = 5;

    public IReadOnlyCollection<T> CardReadOnlyCollection => _items;

    [Range(100, 10000)]
    [SerializeField]
    private float _parabolaParameter = 2000f;

    [SerializeField]
    private float _space = 100f;

    [SerializeField] private bool _rotateItemsToArc = true;

    private RectTransform _containerRect;

    private void Awake()
    {
        _containerRect = GetComponent<RectTransform>();
    }

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

        card.transform.SetParent(_containerRect, false);

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

    public void Refresh() => SetPose();

    private void SetPose()
    {
        var cardCount = _items.Count;
        if (cardCount == 0) return;

        var centerOffset = (cardCount - 1) / 2.0f;

        for (var i = 0; i < cardCount; i++)
        {
            var item = _items[i];
            if (item == null) continue;

            var itemRect = item.transform as RectTransform;
            if (itemRect == null) continue;

            var newPos = CalculateAnchoredPosition(i, centerOffset);
            itemRect.anchoredPosition = newPos;

            if (_rotateItemsToArc)
            {
                var angle = CalculateRotationAngle(i, centerOffset);
                itemRect.localRotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                itemRect.localRotation = Quaternion.identity;
            }
        }
    }

    private Vector2 CalculateAnchoredPosition(int elementIndex, float centerOffset)
    {
        var linear = (elementIndex - centerOffset) * _space;
        var curve = -(linear * linear) / _parabolaParameter;

        if (_isVerticalLayout)
        {
            return new Vector2(curve, -linear);
        }
        else
        {
            return new Vector2(linear, curve);
        }
    }

    private float CalculateRotationAngle(int elementIndex, float centerOffset)
    {
        var x = (elementIndex - centerOffset) * _space;
        var tangent = -2 * x / _parabolaParameter;
        var angle = Mathf.Atan(tangent) * Mathf.Rad2Deg;

        return angle;
    }
}
