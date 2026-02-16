using System.Collections.Generic;
using BitterECS.Core;

public class HandStackControllerDice : HandStackController<KeyValuePair<EcsEntity, DiceProvider>, UIProvider>
{
    public override void Initialize(HandController<KeyValuePair<EcsEntity, DiceProvider>, UIProvider> hand)
    {
        base.Initialize(hand);
        var diceHands = new Loader<HandLoadStackPrefab>(PrefabObjectsPaths.HAND_LOAD_STACK_PREFAB).Prefab().DiceProviders;
        foreach (var dice in diceHands)
        {
            Add(new(dice.ToEntity(), dice), dice.spriteIcon.Prefab());
        }
    }

    public override void Add(KeyValuePair<EcsEntity, DiceProvider> item, UIProvider prefab)
    {
        if (item.Value == null)
        {
            EcsSystems.Run<IHandStackFailAdd>(s => s.ResultStackFailAdd(this));
            return;
        }

        base.Add(item, prefab);
        EcsSystems.Run<IHandStackSucceedAdd>(s => s.ResultStackSucceedAdd(this));
    }

    public override bool DrawToHand()
    {
        var success = base.DrawToHand();

        if (success)
        {
            EcsSystems.Run<IHandStackSucceedExtraction>(s => s.ResultStackSucceedExtraction(this));
            EcsSystems.Run<IHandStackResultInExtractEnded>(s => s.ResultStackInExtractEnded(this));
        }
        else
        {
            EcsSystems.Run<IHandStackFailExtraction>(s => s.ResultStackFailExtraction(this));
        }

        return success;
    }
}
