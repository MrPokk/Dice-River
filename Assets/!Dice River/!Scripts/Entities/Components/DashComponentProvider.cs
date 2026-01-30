using UnityEngine;
using System;
using BitterECS.Integration;

[Serializable]
public struct DashComponent
{
    [Tooltip("Maximum number of dash charges the entity can have.")]
    public int maxCharges;

    [Tooltip("Current number of available dash charges.")]
    public int currentCharges;

    [Tooltip("Time (in seconds) to wait after using a dash before the recharge process begins.")]
    public float rechargeDelay;

    [Tooltip("Time (in seconds) required to restore a single dash charge.")]
    public float rechargeDuration;

    [Tooltip("Duration of the dash in seconds.")]
    public float dashDuration;

    [Tooltip("Speed multiplier applied during the dash.")]
    public float dashSpeedMultiplier;

    [HideInInspector] public float delayTimer;
    [HideInInspector] public float rechargeTimer;
}

public class DashComponentProvider : ProviderEcs<DashComponent>
{
}
