using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;

[RequireComponent(typeof(MovingComponentProvider), typeof(InputComponentProvider))]
public class PlayerProvider : EntitiesProvider
{
    public CharacterController characterController;

    protected override void Registration()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Entity.AddFrameToEvent<IsColliderHit>(new(hit));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ProviderEcs>(out var providerEcs))
            Entity.AddFrameToEvent<IsTriggerColliderEnter>(new(other, providerEcs.Entity));
    }

    public static bool IsPlayerContact(
        EcsEntity entity,
        out ControllerColliderHit hitData,
        out Transform transform,
        out CharacterController controller)
    {
        ref var hitComponent = ref entity.Get<IsColliderHit>();
        hitData = hitComponent.hit;

        if (!hitData.collider.gameObject.TryGetComponent<DiceProvider>(out _))
        {
            transform = null;
            controller = null;
            return false;
        }

        var provider = entity.GetProvider<EntitiesProvider>();
        transform = provider.transform;

        if (!transform.TryGetComponent(out controller))
        {
            return false;
        }

        return true;
    }
}
