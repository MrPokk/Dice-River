using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct MovingComponent
{
    public int baseSpeed;
    [HideInInspector] public int speed;
}

public class MovingComponentProvider : ProviderEcs<MovingComponent>
{
    protected override void Awake()
    {
        Value.speed = Value.baseSpeed;
        base.Awake();
    }
}
