using System.Linq;
using UINotDependence.Core;
using UnityEngine;
using DG.Tweening;

public class UIStackHandElement : UIPopup
{
    public Transform handStackContainer;

    [Header("Layout Settings")]
    [SerializeField] private bool _isVertical;
    [SerializeField] private float _space = 5f;
    [SerializeField] private float _parabola = 2000f;
    [SerializeField] private bool _rotate;

    [Header("Fade Settings")]
    [SerializeField] private bool _useFade = true;
    [SerializeField] private bool _reverseFade = false;
    [Range(0f, 1f)]
    [SerializeField] private float _minAlpha = 0.1f;

    [Header("Sway Settings")]
    [SerializeField] private bool _useSway = true;
    [SerializeField] private float _swayAngle = 5f;
    [SerializeField] private float _swayDuration = 1.2f;

    private HandStackControllerDice _controller;
    private GameObject _lastTopView;

    public void Bind(HandStackControllerDice handStackController)
    {
        if (_controller != null) _controller.OnChanged -= UpdateLayout;
        _controller = handStackController;
        _controller.OnChanged += UpdateLayout;
        _controller.SetContainer(handStackContainer);
    }

    private void OnDestroy()
    {
        if (_controller != null) _controller.OnChanged -= UpdateLayout;
        StopSway();
    }

    private void Update() => UpdateLayout();

    private void UpdateLayout()
    {
        if (_controller == null) return;

        var views = _controller.GetOrderedViews().ToList();
        var count = views.Count;
        if (count == 0)
        {
            StopSway();
            return;
        }

        var offset = (count - 1) / 2f;
        var isLastElement = count > 1;

        for (var i = 0; i < count; i++)
        {
            var view = views[i];
            var rt = view.transform as RectTransform;
            var x = (i - offset) * _space;
            var y = -(x * x) / _parabola;

            rt.anchoredPosition = _isVertical ? new Vector2(y, -x) : new Vector2(x, y);

            if (_useSway && !isLastElement)
            {
                ApplySway(view.gameObject);
            }
            else
            {
                StopSway(view, rt, x);
            }

            rt.SetSiblingIndex(i);

            if (_useFade)
            {
                if (!view.TryGetComponent<CanvasGroup>(out var canvasGroup))
                    canvasGroup = view.gameObject.AddComponent<CanvasGroup>();

                var progress = count > 1 ? (float)i / (count - 1) : 0;

                var startAlpha = _reverseFade ? _minAlpha : 1f;
                var endAlpha = _reverseFade ? 1f : _minAlpha;

                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
            }
        }
    }

    private void ApplySway(GameObject target)
    {
        if (_lastTopView == target) return;

        StopSway();
        _lastTopView = target;

        target.transform.DOLocalRotate(new Vector3(0, 0, _swayAngle), _swayDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(target);
    }

    private void StopSway(UIProvider view, RectTransform rt, float x)
    {
        rt.localRotation = _rotate
            ? Quaternion.Euler(0, 0, Mathf.Atan(-2 * x / _parabola) * Mathf.Rad2Deg)
            : Quaternion.identity;

        if (view.gameObject == _lastTopView)
        {
            view.transform.DOKill();
            _lastTopView = null;
        }
    }

    private void StopSway()
    {
        if (_lastTopView != null)
        {
            _lastTopView.transform.DOKill();
            _lastTopView = null;
        }
    }
}
