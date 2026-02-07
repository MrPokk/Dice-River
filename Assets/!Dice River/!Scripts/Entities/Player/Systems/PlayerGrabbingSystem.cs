using System;
using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabbingSystem : IEcsInitSystem, IEcsFixedRunSystem
{
    public Priority Priority => Priority.Medium;

    private EcsFilter _ecsFilter =
    new EcsFilter<EntitiesPresenter>()
         .WhereProvider<EntitiesProvider>()
         .Include<InputComponent>()
         .Include<FacingComponent>()
         .Exclude<IsGrabbingComponent>();

    private EcsFilter _entityGrab =
    new EcsFilter<EntitiesPresenter>()
       .WhereProvider<EntitiesProvider>()
       .Include<IsGrabbingComponent>()
       .Include<FacingComponent>();

    public void Init()
    {
        var grab = ControllableSystem.Inputs.Playable.Grabbing;
        ControllableSystem.SubscribePerformed(grab, OnGrabbing);
    }

    private void OnGrabbing(InputAction.CallbackContext context)
    {
        if (PlacingObject())
            return;

        TakingObject();
    }

    private bool PlacingObject()
    {
        foreach (var entity in _entityGrab)
        {
            ref var grabbingComponent = ref entity.Get<IsGrabbingComponent>();
            var diceEntity = grabbingComponent.grabbingEntity;

            if (diceEntity == null)
            {
                entity.Remove<IsGrabbingComponent>();
                continue;
            }

            var diceProvider = diceEntity.GetProvider<DiceProvider>();
            var entertainPos = GetPositionTo(entity);

            var isPlacing = DiceInteractionSystem.Placing(entertainPos, diceProvider);
            if (!isPlacing) continue;

            diceProvider.GetComponent<Collider>().enabled = true;
            entity.Remove<IsGrabbingComponent>();
            return true;
        }
        return false;
    }

    private void TakingObject()
    {
        foreach (var entity in _ecsFilter)
        {
            if (entity.Has<IsGrabbingComponent>()) continue;

            var entertainPos = GetPositionTo(entity);
            var dice = DiceInteractionSystem.Extraction(entertainPos);

            if (dice == null) continue;

            entity.Add(new IsGrabbingComponent(dice.Entity));
        }
    }

    public void FixedRun()
    {
        foreach (var entity in _entityGrab)
        {
            ref var grabbingEntity = ref entity.Get<IsGrabbingComponent>().grabbingEntity;
            if (grabbingEntity == null) continue;

            var diceProvider = grabbingEntity.GetProvider<DiceProvider>();
            var transform = entity.GetProvider<EntitiesProvider>().transform;

            diceProvider.transform.position = transform.position + Vector3.up;
            diceProvider.GetComponent<Collider>().enabled = false;
        }
    }

    private static Vector3 GetPositionTo(EcsEntity entity)
    {
        var transform = entity.GetProvider<EntitiesProvider>().transform;
        var facingDir = entity.Get<FacingComponent>().direction;
        var monoGrid = Startup.GridRaft.monoGrid;

        var checkPosition = transform.position + (facingDir.normalized * 0.75f);
        var targetGridPos = monoGrid.ConvertingPosition(checkPosition);
        var entertainPos = monoGrid.ConvertingPosition(targetGridPos);
        return entertainPos;
    }

}

public struct IsGrabbingComponent
{
    public EcsEntity grabbingEntity;

    public IsGrabbingComponent(EcsEntity grabbingEntity)
    {
        this.grabbingEntity = grabbingEntity;
    }
}
