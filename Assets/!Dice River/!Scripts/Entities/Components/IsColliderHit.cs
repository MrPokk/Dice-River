using UnityEngine;

public struct IsColliderHit
{
    public ControllerColliderHit hit;

    public IsColliderHit(ControllerColliderHit hit)
    {
        this.hit = hit;
    }
}
