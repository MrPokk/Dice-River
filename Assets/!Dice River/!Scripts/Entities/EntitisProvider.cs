using System;
using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public class EntitiesProvider : ProviderEcs<EntitiesPresenter>
{
    public SpriteRenderer spriteRenderer;

    private Transform _cameraTransform;
    private Quaternion _lastCameraRotation;

    protected override void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
            _lastCameraRotation = _cameraTransform.rotation;

            if (spriteRenderer != null)
            {
                spriteRenderer.transform.rotation = _lastCameraRotation;
            }
        }

        base.Awake();
    }

    private void LateUpdate()
    {
        if (_cameraTransform == null || spriteRenderer == null) return;

        if (_cameraTransform.rotation != _lastCameraRotation)
        {
            spriteRenderer.transform.rotation = _cameraTransform.rotation;
            _lastCameraRotation = _cameraTransform.rotation;
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        Entity.Add<IsCollisionEnter>(new(collision2D));
    }

    void OnCollisionExit2D(Collision2D collision2D)
    {
        Entity.Remove<IsCollisionEnter>();
    }
}

public class EntitiesPresenter : EcsPresenter
{
    protected override void Registration()
    {
        AddCheckEvent<IsCollisionEnter>();
    }
}
