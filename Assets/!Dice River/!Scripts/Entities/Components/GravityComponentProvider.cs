using System;
using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct GravityComponent
{
    public float gravity;
    public float groundCheckOffset;

    [ReadOnly] public float verticalVelocity;
    [ReadOnly] public bool isGrounded;

    public GravityComponent(float gravity = 80f)
    {
        this.gravity = gravity;
        verticalVelocity = 0f;
        groundCheckOffset = 0;
        isGrounded = true;
    }
}

public class GravityComponentProvider : ProviderEcs<GravityComponent>
{
}
