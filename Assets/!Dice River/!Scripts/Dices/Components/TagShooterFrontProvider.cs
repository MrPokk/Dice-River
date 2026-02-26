using System;
using BitterECS.Integration.Unity;

[Serializable]
public struct TagShooterFront
{
    public float fireRate;
    public int damage;
}

public class TagShooterFrontProvider : ProviderEcs<TagShooterFront> { }
