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
        var spriteRenderer = Value.spriteRenderer;

        if (!spriteRenderer)
        {
            throw new Exception("SpritRender not found");
        }

        if (Camera.main != null)
        {
            Value.cameraTransform = Camera.main.transform;
            Value.lastCameraRotation = Value.cameraTransform.rotation;

            if (spriteRenderer != null)
            {
                spriteRenderer.transform.rotation = Value.lastCameraRotation;
            }
        }
    }

    private void LateUpdate()
    {
        if (Value.cameraTransform == null || Value.spriteRenderer == null) return;

        if (Value.cameraTransform.rotation != Value.lastCameraRotation)
        {
            Value.spriteRenderer.transform.rotation = Value.cameraTransform.rotation;
            Value.lastCameraRotation = Value.cameraTransform.rotation;
        }
    }
}
