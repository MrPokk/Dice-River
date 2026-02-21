using System.Collections.Generic;
using BitterECS.Core;
using Cysharp.Threading.Tasks;

public class CollectedApplyDiceSystem : IHandSucceedAdd
{
    public Priority Priority => Priority.High;

    public UniTask ResultSucceedAdd(HandControllerDice handControllerDice, KeyValuePair<EcsEntity, DiceProvider> item, UIProvider uiProvider)
    {
        GFlow.GState.collectedDiceTypes.Add(uiProvider);

        return UniTask.CompletedTask;
    }
}
