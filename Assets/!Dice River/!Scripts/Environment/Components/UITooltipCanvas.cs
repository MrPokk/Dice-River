using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UITooltipCanvas : MonoBehaviour
{
    public UITooltipPopup tooltipPopup;

    [Header("Settings")]
    [SerializeField] private float _scaleFactor = 0.01f;
    [SerializeField] private int _orderLayer = 10;

    private Canvas _canvas;
    private Camera _mainCamera;
    private RectTransform _childRect;
    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
        _canvas = GetComponent<Canvas>();
        _mainCamera = Camera.main;

        SetupCanvas();
        InitializePopup();
    }

    private void InitializePopup()
    {
        if (tooltipPopup == null)
            tooltipPopup = GetComponentInChildren<UITooltipPopup>();

        if (tooltipPopup != null)
        {
            _childRect = tooltipPopup.transform as RectTransform;
            SetupChildRect();
        }
    }

    private void SetupCanvas()
    {
        _canvas.renderMode = RenderMode.WorldSpace;
        _canvas.sortingOrder = _orderLayer;
        _canvas.worldCamera = _mainCamera;
        _transform.localScale = Vector3.one * _scaleFactor;
    }

    private void LateUpdate()
    {
        if (_mainCamera == null) _mainCamera = Camera.main;
        if (_mainCamera == null) return;
        SetupChildRect();
        RotateToCamera();
    }

    private void RotateToCamera()
    {
        Vector3 direction = _mainCamera.transform.position - _transform.position;
        _transform.rotation = Quaternion.LookRotation(-direction);
    }

    private void SetupChildRect()
    {
        if (_childRect == null) return;

        _childRect.anchorMin = _childRect.anchorMax = _childRect.pivot = new Vector2(0.5f, 0.5f);
        _childRect.anchoredPosition3D = Vector3.zero;
    }

    private void OnValidate()
    {
        if (_canvas == null) _canvas = GetComponent<Canvas>();
        transform.localScale = Vector3.one * _scaleFactor;

        if (tooltipPopup == null) tooltipPopup = GetComponentInChildren<UITooltipPopup>();
        if (tooltipPopup != null && _childRect == null) _childRect = tooltipPopup.transform as RectTransform;

        SetupChildRect();
    }
}
