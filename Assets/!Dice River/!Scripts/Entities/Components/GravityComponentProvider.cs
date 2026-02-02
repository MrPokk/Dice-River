using System;
using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct GravityComponent
{
    public float gravity;
    public LayerMask groundMask;

    [HideInInspector] public float verticalVelocity;
    [HideInInspector] public float groundCheckOffset;
    [HideInInspector] public bool isGrounded;

    public GravityComponent(float gravity = 80f)
    {
        this.gravity = gravity;
        groundMask = 0;
        verticalVelocity = 0f;
        groundCheckOffset = -0.55f;
        isGrounded = false;
    }
}

public class GravityComponentProvider : ProviderEcs<GravityComponent>
{
}
