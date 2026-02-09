using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct MovingComponent
{
    public int originalVelocity;
    [ReadOnly] public int velocity;
    [ReadOnly] public float jumpVelocityX;
    public void Reset() => velocity = originalVelocity;
}

public class MovingComponentProvider : ProviderEcs<MovingComponent>
{
    private void Start()
    {
        Value.Reset();
    }
}
