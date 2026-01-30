using System;
using BitterECS.Integration;
using UnityEngine;

[Serializable]
public struct InputComponent
{
    [HideInInspector] public Vector2 currentInput;
}
public class InputComponentProvider : ProviderEcs<InputComponent> { }
