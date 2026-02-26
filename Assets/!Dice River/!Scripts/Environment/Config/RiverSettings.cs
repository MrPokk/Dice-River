using System.Collections.Generic;
using BitterECS.Integration.Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "RiverSettings", menuName = "Settings/RiverGlobal")]
public class RiverSettings : ScriptableObject
{
    public RiverDecorationSettings decorationSettings;
    public RiverHazardSettings hazardSettings;
    public RiverPickupSettings pickupSettings;
}
