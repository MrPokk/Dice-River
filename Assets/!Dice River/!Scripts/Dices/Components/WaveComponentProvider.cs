

using BitterECS.Integration;
using UnityEngine;

public struct WaveComponent
{
    [ReadOnly] public float currentSinkOffset;
    [ReadOnly] public bool isDepressed;
    [HideInInspector] public Vector3 initialRollLocalPos;
    [HideInInspector] public Vector3 initialSideLocalPos;
    [HideInInspector] public bool initialized;
}

public class WaveComponentProvider : ProviderEcs<WaveComponent>
{
}
