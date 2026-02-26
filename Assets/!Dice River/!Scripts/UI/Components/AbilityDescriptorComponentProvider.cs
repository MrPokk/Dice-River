using System;
using BitterECS.Integration.Unity;
using UnityEngine;

[Serializable]
public struct AbilityDescriptorComponent
{
    [TextArea] public string description;
}

public class AbilityDescriptorComponentProvider : ProviderEcs<AbilityDescriptorComponent> { }
