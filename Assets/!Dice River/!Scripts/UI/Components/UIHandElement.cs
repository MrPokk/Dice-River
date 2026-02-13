using System.Linq;
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

    public void Bind(HandControllerDice handController)
    {
        if (_controller != null) _controller.OnChanged -= UpdateLayout;
        _controller = handController;
        _controller.OnChanged += UpdateLayout;
        _controller.SetContainer(handContainer);
    }

    private void OnDestroy()
    {
        if (_controller != null) _controller.OnChanged -= UpdateLayout;
    }

    private void Update() => UpdateLayout();

    private void UpdateLayout()
    {
        if (_controller == null) return;

        var views = _controller.GetViews().ToList();
        var count = views.Count;
        if (count == 0) return;

        var centerOffset = (count - 1) / 2.0f;

        for (var i = 0; i < count; i++)
        {
            var view = views[i];
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
