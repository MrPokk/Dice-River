using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

public struct IsTriggerColliderEnter
{
    public EcsEntity entity;
    public Collider collision;
    public IsTriggerColliderEnter(Collider collision, EcsEntity entity)
    {
        this.collision = collision;
        this.entity = entity;
    }
}
