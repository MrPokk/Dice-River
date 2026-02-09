using BitterECS.Integration;
using UnityEngine;

[RequireComponent(typeof(MovingComponentProvider), typeof(InputComponentProvider))]
public class PlayerProvider : EntitiesProvider
{
    public CharacterController characterController;
    protected override void Awake()
    {
        characterController = GetComponent<CharacterController>();
        base.Awake();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Entity.AddFrameToEvent(new IsColliderHit(hit));
    }
}
