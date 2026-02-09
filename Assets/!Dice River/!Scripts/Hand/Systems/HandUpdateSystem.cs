using System;
using BitterECS.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HandUpdateSystem : IHandResultInExtractEnded
{
    public Priority Priority => Priority.High;

    public async UniTask ResultInExtractEnded(HandControllerDice hand)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(hand.timeRefreshSecond));

        var prefabDice = new Loader<DiceProvider>(DicesPaths.BASE_DICE).Prefab();

        for (var i = 0; i < 3; i++)
        {
            hand.Add(prefabDice.NewEntity(), prefabDice.spriteIcon.Prefab());
        }
    }
}
