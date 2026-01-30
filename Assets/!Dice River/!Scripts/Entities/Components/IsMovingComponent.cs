using UnityEngine;

public struct IsMovingComponent
{
    public Vector2 direction;
    public int stepsRemaining;

    public IsMovingComponent(Vector2 dir, int steps)
    {
        direction = dir;
        stepsRemaining = steps;
    }
}
