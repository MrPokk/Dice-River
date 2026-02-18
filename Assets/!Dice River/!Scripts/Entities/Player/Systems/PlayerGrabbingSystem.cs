using BitterECS.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrabbingSystem : IEcsInitSystem, IEcsFixedRunSystem
{
    public Priority Priority => Priority.Medium;

    private const int GrabLayerOffset = 10;

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

            if (diceEntity == null || !diceEntity.IsAlive)
            {
                entity.Remove<IsGrabbingComponent>();
                continue;
            }

            var diceProvider = diceEntity.GetProvider<DiceProvider>();
            var entertainPos = GetPositionTo(entity);

            var isPlacing = DiceInteractionSystem.Placing(entertainPos, diceProvider);
            if (!isPlacing) continue;

            diceProvider.GetComponent<Collider>().enabled = true;

            diceProvider.spriteSide.SetState(DiceVisualState.Default);
            diceProvider.spriteSide.ToggleRipple();
            ChangeSortingOrder(diceProvider, -GrabLayerOffset);

            entity.Remove<IsGrabbingComponent>();
            return true;
        }
        return false;
    }

    private void TakingObject()
    {
        foreach (var entity in _ecsFilter)
        {
            var entertainPos = GetPositionTo(entity);
            var diceProvider = DiceInteractionSystem.Extraction(entertainPos);

            if (diceProvider == null) continue;

            diceProvider.spriteSide.SetState(DiceVisualState.Lifted);
            diceProvider.spriteSide.ToggleRipple();
            entity.Add(new IsGrabbingComponent(diceProvider.Entity));
            var transform = entity.GetProvider<EntitiesProvider>().transform;

            diceProvider.GetComponent<Collider>().enabled = false;
            diceProvider.transform.position = transform.position + Vector3.up;

            ChangeSortingOrder(diceProvider, GrabLayerOffset);
        }
    }

    public void FixedRun()
    {
        foreach (var entity in _entityGrab)
        {
            ref var grabbingComponent = ref entity.Get<IsGrabbingComponent>();
            var grabbingEntity = grabbingComponent.grabbingEntity;

            if (!grabbingEntity.IsAlive) continue;

            var diceProvider = grabbingEntity.GetProvider<DiceProvider>();
            var transform = entity.GetProvider<EntitiesProvider>().transform;

            diceProvider.transform.position = transform.position + Vector3.up;
        }
    }

    private void ChangeSortingOrder(MonoBehaviour provider, int offset)
    {
        if (provider == null) return;
        var renderers = provider.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (var spriteRenderer in renderers)
        {
            spriteRenderer.sortingOrder += offset;
        }
    }

    private static Vector3 GetPositionTo(EcsEntity entity)
    {
        var transform = entity.GetProvider<EntitiesProvider>().transform;
        var facingDir = entity.Get<FacingComponent>();
        var monoGrid = Startup.GridRaft.monoGrid;

        var checkPosition = transform.position + (facingDir.direction.normalized * 0.75f);
        var targetGridPos = monoGrid.ConvertingPosition(checkPosition);
        var entertainPos = monoGrid.ConvertingPosition(targetGridPos);
        return entertainPos;
    }
}
