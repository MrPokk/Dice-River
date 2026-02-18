using System;
using BitterECS.Core;
using InGame.Script.Component_Sound;
using UnityEngine;

public class PlayerSoundSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Low;

    private EcsEvent _ecsEventHealth = new EcsEvent<EntitiesPresenter>()
        .SubscribeWhereEntity<IsHealthChanging>(e => e.Has<InputComponent>(), added: OnHealth);

    private EcsEvent _ecsEventGround = new EcsEvent<EntitiesPresenter>()
          .SubscribeWhereEntity<IsGroundedSound>(e => e.Has<InputComponent>(), added: OnGroundSplash, removed: OnGroundExit);

    private static void OnGroundExit(EcsEntity entity)
    {
        SoundController.PlaySoundRandomPitch(SoundType.WaterExit, volume: 0.15f);
    }

    private static void OnGroundSplash(EcsEntity entity)
    {
        SoundController.PlaySoundRandomPitch(SoundType.WaterSplash, volume: 0.15f);
    }

    private static void OnHealth(EcsEntity entity)
    {
        SoundController.PlaySoundRandomPitch(SoundType.PlayerDamaging, volume: 0.15f);
    }
}
