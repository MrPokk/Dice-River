using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct DescriptorComponent
{
    [TextArea] public string description;
}

public class DescriptorComponentProvider : ProviderEcs<DescriptorComponent> { }
