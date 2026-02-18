using System.Collections.Generic;
using UINotDependence.Core;
using UnityEngine;

public class UIHandElement : UIPopup
{
    public Transform handContainer;

    [Header("Layout Settings")]
    [SerializeField] private bool _isVerticalLayout;
    [SerializeField] private float _space = 100f;
    [Range(100, 10000)]
    [SerializeField] private float _parabolaParameter = 2000f;
    [SerializeField] private bool _rotateItemsToArc = true;

    private HandControllerDice _controller;
    private readonly List<RectTransform> _rectCache = new();

    public void Bind(HandControllerDice handController)
    {
        if (_controller != null) _controller.OnChanged -= RefreshCache;

        _controller = handController;
        _controller.OnChanged += RefreshCache;
        _controller.SetContainer(handContainer);

        RefreshCache();
    }

    private void OnDestroy()
    {
        if (_controller != null) _controller.OnChanged -= RefreshCache;
    }

    private void RefreshCache()
    {
        if (_controller == null) return;

        _rectCache.Clear();
        foreach (var view in _controller.GetViews())
        {
            if (view != null)
            {
                _rectCache.Add(view.GetComponent<RectTransform>());
            }
        }
        UpdateLayout();
    }

    private void Update() => UpdateLayout();

    private void UpdateLayout()
    {
        var count = _rectCache.Count;
        if (count == 0) return;

        var centerOffset = (count - 1) * 0.5f;
        var invParabola = 1f / _parabolaParameter;

        for (var i = 0; i < count; i++)
        {
            RectTransform rect = _rectCache[i];
            if (rect == null) continue;

            var linear = (i - centerOffset) * _space;
            var curve = -(linear * linear) * invParabola;

            if (_isVerticalLayout)
                rect.anchoredPosition = new Vector2(curve, -linear);
            else
                rect.anchoredPosition = new Vector2(linear, curve);

            if (_rotateItemsToArc)
            {
                var tangent = -2f * linear * invParabola;
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
