using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct NameComponent
{
    [TextArea] public string value;
}


public class NameComponentProvider : ProviderEcs<NameComponent> { }
