using BitterECS.Core;
using Cysharp.Threading.Tasks;

public class CollectedApplyDiceSystem : IHandSucceedAdd
{
    public Priority Priority => Priority.High;

    public UniTask ResultSucceedAdd(HandControllerDice handControllerDice, EcsEntity entity, UIProvider uiProvider)
    {
        var diceTypeUI = entity.GetProvider<DiceProvider>().spriteIcon.Prefab();
        StartupGameplay.GState.collectedDiceTypes.Add(diceTypeUI);
        return UniTask.CompletedTask;
    }
}
