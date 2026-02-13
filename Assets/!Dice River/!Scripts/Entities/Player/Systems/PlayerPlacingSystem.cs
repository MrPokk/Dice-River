using System;
using BitterECS.Core;
using UnityEngine;
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
        var set = ControllableSystem.Inputs.Playable.BasicAttack;
        ControllableSystem.SubscribePerformed(set, OnPlacing);
    }

    private void OnPlacing(InputAction.CallbackContext context)
    {
        foreach (var entity in _ecsFilter)
        {
            var transform = entity.GetProvider<EntitiesProvider>().transform;
            var facingDir = entity.Get<FacingComponent>().direction;
            var monoGrid = Startup.GridRaft.monoGrid;

            var checkPosition = transform.position + (facingDir.normalized * 0.75f);
            var targetGridPos = monoGrid.ConvertingPosition(checkPosition);

            if (DiceInteractionSystem.IsPlacing(targetGridPos))
            {
                if (Startup.HandControllerDice.ExtractToFirst(out var entityToHand))
                {
                    if (entityToHand != null)
                    {
                        var diceProviderPrefab = entityToHand.GetProvider<DiceProvider>();
                        DiceInteractionSystem.InstantiateObject(targetGridPos, diceProviderPrefab, out _);

                        entityToHand.Destroy();
                    }
                }
            }
        }
    }
}
