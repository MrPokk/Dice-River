using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HandController<TData, TView> : MonoBehaviour where TView : MonoBehaviour
{
    [Header("Layout Settings")]
    [SerializeField] private bool _isVerticalLayout;
    [SerializeField] private float _space = 100f;
    [Range(100, 10000)]
    [SerializeField] private float _parabolaParameter = 2000f;
    [SerializeField] private bool _rotateItemsToArc = true;

    private readonly List<TData> _dataItems = new();
    private readonly Dictionary<TData, TView> _viewMap = new();

    private RectTransform _containerRect;

    public IReadOnlyCollection<TData> Items => _dataItems;

    private void Awake()
    {
        _containerRect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdateLayout();
    }

    public void Add(TData data, TView viewPrefab)
    {
        if (data == null || _viewMap.ContainsKey(data)) return;

        var viewInstance = Instantiate(viewPrefab, _containerRect);

        _dataItems.Add(data);
        _viewMap.Add(data, viewInstance);

        UpdateLayout();
    }

    public void Remove(TData data)
    {
        if (_viewMap.TryGetValue(data, out var view))
        {
            Destroy(view.gameObject);
            _viewMap.Remove(data);
            _dataItems.Remove(data);
            UpdateLayout();
        }
    }

    private void UpdateLayout()
    {
        var count = _dataItems.Count;
        if (count == 0) return;

        var centerOffset = (count - 1) / 2.0f;

        for (var i = 0; i < count; i++)
        {
            var data = _dataItems[i];
            var view = _viewMap[data];

            if (view == null) continue;

            var rect = view.transform as RectTransform;

            var linear = (i - centerOffset) * _space;
            var curve = -(linear * linear) / _parabolaParameter;

            rect.anchoredPosition = _isVerticalLayout
                ? new Vector2(curve, -linear)
                : new Vector2(linear, curve);

            if (_rotateItemsToArc)
            {
                var tangent = -2 * linear / _parabolaParameter;
                var angle = Mathf.Atan(tangent) * Mathf.Rad2Deg;
                rect.localRotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                rect.localRotation = Quaternion.identity;
            }
        }
    }
}
