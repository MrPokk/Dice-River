using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct DescriptionComponent
{
    [TextArea] public string description;
}

public class DescriptionComponentProvider : ProviderEcs<DescriptionComponent> { }
