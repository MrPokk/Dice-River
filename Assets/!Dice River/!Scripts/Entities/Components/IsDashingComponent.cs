using UnityEngine;

public struct IsDashingComponent
{
    public Vector2 direction;
    public float remainingTime;

    public IsDashingComponent(float remainingTime, Vector2 direction) : this()
    {
        this.remainingTime = remainingTime;
        this.direction = direction;
    }
}
