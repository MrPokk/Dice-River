using System;
using BitterECS.Integration;

[Serializable]
public struct JumpComponent
{
    public float jumpHeight;
    public float jumpLength;
}

public struct JumpEvent
{

}

public class JumpComponentProvider : ProviderEcs<JumpComponent>
{

}
