using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightObjectController : MonoBehaviour
{
    [Header("Intensity Settings")]
    [SerializeField] private bool intensityEnabled = false;
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float intensitySpeed = 1f;

    [Header("Rotation Settings")]
    [SerializeField] private bool rotationEnabled = false;
    [SerializeField] private Vector3 rotationRange = new Vector3(0, 0, 15f);
    [SerializeField] private float rotationSpeed = 1f;

    [Header("Scale Settings")]
    [SerializeField] private bool scaleEnabled = false;
    [SerializeField] private Vector3 minScale = new Vector3(0.8f, 0.8f, 1f);
    [SerializeField] private Vector3 maxScale = new Vector3(1.2f, 1.2f, 1f);
    [SerializeField] private float scaleSpeed = 1f;

    [Header("Camera Tracing")]
    [SerializeField] private bool cameraTracing;
    private Light2D lightComponent;
    private Vector3 initialRotation;

    private void Start()
    {
        lightComponent = GetComponent<Light2D>();
        initialRotation = transform.localEulerAngles;

        if (cameraTracing)
        {
            var camera = Camera.main;
            transform.SetParent(camera.transform);
        }
    }

    private void Update()
    {
        UpdateIntensity();
        UpdateRotation();
        UpdateScale();
    }

    private void UpdateIntensity()
    {
        if (lightComponent == null) return;
        if (!intensityEnabled) return;
        float t = (Mathf.Sin(Time.time * intensitySpeed) + 1f) / 2f;
        lightComponent.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }

    private void UpdateRotation()
    {
        if (!rotationEnabled) return;
        float t = Mathf.Sin(Time.time * rotationSpeed);
        transform.localEulerAngles = initialRotation + (rotationRange * t);
    }

    private void UpdateScale()
    {
        if (!scaleEnabled) return;
        float t = (Mathf.Sin(Time.time * scaleSpeed) + 1f) / 2f;
        transform.localScale = Vector3.Lerp(minScale, maxScale, t);
    }
}
