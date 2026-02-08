using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public class RotationToCameraComponent
{
    public SpriteRenderer spriteRenderer;

    [ReadOnly] public Transform cameraTransform;
    [ReadOnly] public Quaternion lastCameraRotation;
}

public class RotationToCameraComponentProvider : ProviderEcs<RotationToCameraComponent>
{
    protected override void Awake()
    {
        base.Awake();
        var spriteRenderer = _value.spriteRenderer;

        if (!spriteRenderer)
        {
            throw new Exception("SpritRender not found");
        }

        if (Camera.main != null)
        {
            _value.cameraTransform = Camera.main.transform;
            _value.lastCameraRotation = _value.cameraTransform.rotation;

            if (spriteRenderer != null)
            {
                spriteRenderer.transform.rotation = _value.lastCameraRotation;
            }
        }
    }

    private void LateUpdate()
    {
        if (_value.cameraTransform == null || _value.spriteRenderer == null) return;

        if (_value.cameraTransform.rotation != _value.lastCameraRotation)
        {
            _value.spriteRenderer.transform.rotation = _value.cameraTransform.rotation;
            _value.lastCameraRotation = _value.cameraTransform.rotation;
        }
    }
}
