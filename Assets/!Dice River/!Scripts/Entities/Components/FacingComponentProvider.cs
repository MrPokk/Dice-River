using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct FacingComponent
{
    public Vector3 direction;
}

public class FacingComponentProvider : ProviderEcs<FacingComponent>
{

}
