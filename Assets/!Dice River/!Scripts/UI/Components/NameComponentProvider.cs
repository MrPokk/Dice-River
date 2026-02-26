using System;
using BitterECS.Integration.Unity;
using UnityEngine;

[Serializable]
public struct NameComponent
{
    [TextArea] public string value;
}


public class NameComponentProvider : ProviderEcs<NameComponent> { }
