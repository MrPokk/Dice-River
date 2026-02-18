using System.Collections.Generic;
using BitterECS.Core;
using Cysharp.Threading.Tasks;
using InGame.Script.Component_Sound;

public class HandSoundSystem : IHandSucceedAdd
{
    public Priority Priority => Priority.High;

    public UniTask ResultSucceedAdd(
        HandControllerDice handControllerDice,
        KeyValuePair<EcsEntity, DiceProvider> item,
        UIProvider uiProvider)
    {
        SoundManager.PlaySoundRandomPitch(SoundType.GiveOutDice, volume: 0.2f);
        return UniTask.CompletedTask;
    }
}
