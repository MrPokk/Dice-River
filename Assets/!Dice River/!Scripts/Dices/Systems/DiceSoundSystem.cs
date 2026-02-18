using System;
using BitterECS.Core;
using InGame.Script.Component_Sound;

public class DiceSoundSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Low;

    private EcsEvent _ecsEventRoll = new EcsEvent<DicePresenter>()
    .Subscribe<IsRollingProcess>(added: OnRollSound);

    private EcsEvent _ecsEventInstantiate = new EcsEvent<DicePresenter>()
    .Subscribe<IsPlacingEvent>(added: OnInstantiateSound);

    private EcsEvent _ecsEventDestroy = new EcsEvent<DicePresenter>()
         .Subscribe<IsDestroy>(added: OnDiceDestroy);

    private static void OnInstantiateSound(EcsEntity entity)
    {
        if (!entity.IsProviding)
            return;

        SoundController.PlaySoundRandomPitch(SoundType.SplashInDice);
    }

    private static void OnRollSound(EcsEntity entity)
    {
        if (!entity.IsProviding)
            return;

        SoundController.PlaySoundRandomPitch(SoundType.Roll);
    }

    private static void OnDiceDestroy(EcsEntity entity)
    {
        if (!entity.IsProviding)
            return;

        // SoundManager.PlaySound(SoundType.Break, volume: 0.5f);
    }

}
