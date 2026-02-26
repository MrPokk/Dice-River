using System;
using BitterECS.Integration.Unity;
using UnityEngine;

[Serializable]
public struct InputComponent
{
    [HideInInspector] public Vector2 currentInput;
}
public class InputComponentProvider : ProviderEcs<InputComponent> { }
