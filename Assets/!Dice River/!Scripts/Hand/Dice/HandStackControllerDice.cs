using BitterECS.Core;
using UnityEngine;

public class HandStackControllerDice : HandStackController<EcsEntity, UIProvider>
{
    public override void Initialize(HandController<EcsEntity, UIProvider> hand)
    {
        base.Initialize(hand);
        var diceHands = new Loader<HandLoadStackPrefab>(PrefabObjectsPaths.HAND_LOAD_STACK_PREFAB).Prefab().DiceProviders;
        Debug.Log(diceHands.Count);
        foreach (var dice in diceHands)
        {
            Add(dice.NewEntity(), dice.spriteIcon.Prefab());
        }
    }
}
