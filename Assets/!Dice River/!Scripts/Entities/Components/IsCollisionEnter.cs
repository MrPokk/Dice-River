using UnityEngine;

internal struct IsCollisionEnter
{
    public Collision2D collision;
    public IsCollisionEnter(Collision2D collision)
    {
        this.collision = collision;
    }
}
