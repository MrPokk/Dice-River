using System;
using System.Linq;
using BitterECS.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HandUpdateSystem : IHandResultInExtractEnded, IHandSucceedAdd
{
    public Priority Priority => Priority.High;

    public async UniTask ResultInExtractEnded(HandControllerDice hand)
    {
        //    await UniTask.Delay(TimeSpan.FromSeconds(hand.timeRefreshSecond));

        var max = hand.maxCountDice;
        for (var i = 0; i < max; i++)
        {
            hand.handStackController.DrawToHand();
        }
    }

    public async UniTask ResultSucceedAdd(HandControllerDice hand)
    {
        if (!hand.Items.Any())
        {
            return;
        }

        var max = hand.maxCountDice;
        for (var i = 0; i < max; i++)
        {
            hand.handStackController.DrawToHand();
        }
    }
}
