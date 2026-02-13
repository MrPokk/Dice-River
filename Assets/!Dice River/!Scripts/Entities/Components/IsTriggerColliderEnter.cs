using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public struct IsTriggerColliderEnter
{
    public EcsEntity entityHit;
    public Collider collision;
    public IsTriggerColliderEnter(Collider collision, EcsEntity entity)
    {
        this.collision = collision;
        this.entityHit = entity;
    }
}
