using System;
using BitterECS.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetDiceSystem : IEcsInitSystem, IEcsFixedRunSystem
{
    public Priority Priority => Priority.FIRST_TASK;

    private EcsFilter _ecsFilter =
    Build.For<EntitiesPresenter>()
         .Filter()
         .WhereProvider<EntitiesProvider>()
         .Include<InputComponent>()
         .Include<FacingComponent>();

    private Transform _selector;

    public void Init()
    {
        var set = ControllableSystem.Inputs.Playable.BasicAttack;
        ControllableSystem.SubscribePerformed(set, SetDice);

        var selectorPrefab = new Loader<GameObject>(DicesPaths.SELECTOR_DICE).GetInstance();
        _selector = selectorPrefab.transform;
    }

    public void FixedRun()
    {
        foreach (var entity in _ecsFilter)
        {
            var transform = entity.GetProvider<EntitiesProvider>().transform;

            var facingDir = (Vector3)entity.Get<FacingComponent>().direction;

            var targetPosition = transform.position + facingDir;
            var gridPosition = Startup.GridRaft.ConvertingPosition(targetPosition);

            _selector.position = Startup.GridRaft.ConvertingPosition(gridPosition);
        }
    }

    private void SetDice(InputAction.CallbackContext context)
    {
        foreach (var entity in _ecsFilter)
        {
            var transform = entity.GetProvider<EntitiesProvider>().transform;
            var facingDir = (Vector3)entity.Get<FacingComponent>().direction;

            var targetPosition = transform.position + facingDir;
            var dicePrefab = new Loader<DiceProvider>(DicesPaths.GENERAL_DICE).GetPrefab();
            var gridPosition = Startup.GridRaft.ConvertingPosition(targetPosition);

            Startup.SpawnDiceRaft(gridPosition, dicePrefab, out _);
        }
    }
}
