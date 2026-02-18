using BitterECS.Core;
using UnityEngine.InputSystem;

public class PlayerPlacingSystem : IEcsInitSystem
{
    public Priority Priority => Priority.High;

    private EcsFilter _ecsFilter =
    Build.For<EntitiesPresenter>()
         .Filter()
         .WhereProvider<EntitiesProvider>()
         .Include<InputComponent>()
         .Include<FacingComponent>();

    public void Init()
    {
        var set = ControllableSystem.Inputs.Playable.Interactable;
        ControllableSystem.SubscribePerformed(set, OnPlacing);
    }

    private void OnPlacing(InputAction.CallbackContext context)
    {
        foreach (var entity in _ecsFilter)
        {
            var transform = entity.GetProvider<EntitiesProvider>().transform;
            var facingDir = entity.Get<FacingComponent>();
            var monoGrid = Startup.GridRaft.monoGrid;

            var checkPosition = transform.position + (facingDir.direction.normalized * 0.75f);
            var targetGridPos = monoGrid.ConvertingPosition(checkPosition);

            if (DiceInteractionSystem.IsPlacing(targetGridPos))
            {
                if (Startup.HandControllerDice == null) return;

                if (Startup.HandControllerDice.ExtractToFirst(out var diceItem))
                {
                    var diceProviderPrefab = diceItem.Value;

                    if (diceProviderPrefab != null)
                    {
                        DiceInteractionSystem.InstantiateObject(targetGridPos, diceProviderPrefab, out _);

                        diceItem.Key.Destroy();
                    }
                }
            }
        }
    }
}
