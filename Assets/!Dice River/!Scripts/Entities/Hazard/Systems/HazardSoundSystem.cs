using System;
using BitterECS.Core;
using BitterECS.Integration;
using InGame.Script.Component_Sound;
using UnityEngine;

public class HazardSoundSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Low;

    private EcsEvent _ecsEventBomb =
        new EcsEvent<EntitiesPresenter>(Priority.High)
            .SubscribeWhereEntity<IsSoundPlay>(e => e.Has<TagBombHazard>(), added: OnBombSound);
    private EcsEvent _ecsEventRock =
        new EcsEvent<EntitiesPresenter>(Priority.High)
            .SubscribeWhereEntity<IsSoundPlay>(e => e.Has<TagRockHazard>(), added: OnRockSound);

    private static void OnRockSound(EcsEntity entity)
    {
        SoundManager.PlaySoundRandomPitch(SoundType.DamageInDice);
    }

    private static void OnBombSound(EcsEntity entity)
    {
        SoundManager.PlaySound(SoundType.DamageInDice);
        SoundManager.PlaySoundRandomVolumeAndPitch(SoundType.Explosion, 0.7f, 1, 0.55f, 1.15f);
    }
}
