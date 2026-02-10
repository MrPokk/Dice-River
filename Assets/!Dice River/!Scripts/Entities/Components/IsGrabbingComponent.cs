using BitterECS.Core;
using UnityEngine;

public struct IsGrabbingComponent
{
    public EcsEntity grabbingEntity;

    public IsGrabbingComponent(EcsEntity grabbingEntity)
    {
        this.grabbingEntity = grabbingEntity;
    }
}
