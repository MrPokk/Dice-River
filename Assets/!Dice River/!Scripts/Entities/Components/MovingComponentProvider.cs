using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct MovingComponent
{
    public int baseSpeed;
    [ReadOnly] public int speed;
    public void Reset() => speed = baseSpeed;
}

public class MovingComponentProvider : ProviderEcs<MovingComponent>
{
    protected override void Awake()
    {
        Value.speed = Value.baseSpeed;
        base.Awake();
    }
}
