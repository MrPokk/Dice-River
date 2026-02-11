using System;
using BitterECS.Integration;

[Serializable]
public struct TagShooterFront
{
    public float fireRate;
    public int damage;
}

public class TagShooterFrontProvider : ProviderEcs<TagShooterFront> { }
