using BitterECS.Integration;
using UnityEngine;

[RequireComponent(typeof(MovingComponentProvider), typeof(InputComponentProvider))]
public class PlayerProvider : EntitiesProvider
{
    protected override void Awake()
    {
        base.Awake();
    }
}
