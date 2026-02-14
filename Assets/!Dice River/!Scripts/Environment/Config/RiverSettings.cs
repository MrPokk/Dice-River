using System.Collections.Generic;
using BitterECS.Integration;
using UnityEngine;

[CreateAssetMenu(fileName = "RiverSettings", menuName = "Settings/RiverGlobal")]
public class RiverSettings : ScriptableObject
{
    public RiverDecorationSettings decorationSettings;
    public RiverHazardSettings hazardSettings;
    public RiverPickupSettings pickupSettings;
}
