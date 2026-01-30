using UnityEngine;

public struct IsDamageComponent
{
    public Vector2 direction;
    public int damage;

    public IsDamageComponent(int damage, Vector2 impulse)
    {
        direction = impulse;
        this.damage = damage;
    }
}
