using System;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UITooltipCanvas : MonoBehaviour
{
    private Canvas _canvas;
    public UITooltipPopup tooltipPopup;

    [Header("Settings")]
    [SerializeField] private float _scaleFactor = 0.01f;
    [SerializeField] private int _orderLayer = 10;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        SetupCanvas();
    }

    private void LateUpdate()
    {
        if (_canvas.worldCamera == null)
        {
            _canvas.worldCamera = Camera.main;
        }

        CenterChild();

        RotationToCamera();
    }

    private void RotationToCamera()
    {
        if (_canvas.worldCamera != null)
        {
            var directionToCamera = _canvas.worldCamera.transform.position - transform.position;

            transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }
    }

    private void SetupCanvas()
    {
        _canvas.renderMode = RenderMode.WorldSpace;
        _canvas.sortingOrder = _orderLayer;
        ApplyScale();
    }

    private void ApplyScale()
    {
        if (transform.localScale.x != _scaleFactor)
        {
            transform.localScale = new Vector3(_scaleFactor, _scaleFactor, _scaleFactor);
        }
    }

    private void CenterChild()
    {
        if (transform.childCount > 0)
        {
            tooltipPopup = transform.GetComponentInChildren<UITooltipPopup>();
            if (tooltipPopup == null)
            {
                throw new Exception("Tooltip NOT set");
            }

            var childRect = tooltipPopup.transform as RectTransform;

            if (childRect != null)
            {
                childRect.anchorMin = new Vector2(0.5f, 0.5f);
                childRect.anchorMax = new Vector2(0.5f, 0.5f);
                childRect.pivot = new Vector2(0.5f, 0.5f);

                childRect.anchoredPosition3D = Vector3.zero;
            }
        }
    }

    private void OnValidate()
    {
        if (_canvas == null) _canvas = GetComponent<Canvas>();
        ApplyScale();
        CenterChild();
    }
}
