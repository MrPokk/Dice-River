using System;
using BitterECS.Core;
using UnityEngine.InputSystem;

public class PlayerSetDiceSystem : IEcsInitSystem
{
    public Priority Priority => Priority.FIRST_TASK;

    private EcsFilter _ecsFilter =
    Build.For<EntitiesPresenter>()
         .Filter()
         .WhereProvider<EntitiesProvider>()
         .Include<InputComponent>()
         .Include<FacingComponent>();

    public void Init()
    {
        var set = ControllableSystem.Inputs.Playable.BasicAttack;
        ControllableSystem.SubscribePerformed(set, SetDice);
    }

    private void SetDice(InputAction.CallbackContext context)
    {
        foreach (var entity in _ecsFilter)
        {
            var transform = entity.GetProvider<EntitiesProvider>().transform;
            var facingDir = entity.Get<FacingComponent>().direction;

            var targetPosition = transform.position + facingDir;

            var dicePrefab = new Loader<DiceProvider>(DicesPaths.TEST_DICE).GetPrefab();
            DiceSetterSystem.SpawnDiceRaft(targetPosition, dicePrefab, out _);
        }
    }
}
