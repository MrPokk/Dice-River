using BitterECS.Core;
using Cysharp.Threading.Tasks;

public class HandUpdateSystem : IHandSucceedRemove, IHandSucceedExtraction, IHandStackSucceedAdd
{
    public Priority Priority => Priority.High;

    public async UniTask ResultStackSucceedAdd(HandStackControllerDice stack)
    {
        OnAddHand((HandControllerDice)stack.hand);
    }

    public async UniTask ResultSucceedExtraction(HandControllerDice hand)
    {
        OnAddHand(hand);
    }

    public async UniTask ResultSucceedRemove(HandControllerDice hand)
    {
        OnAddHand(hand);
    }

    private static void OnAddHand(HandControllerDice hand)
    {
        var currentCount = hand.Items.Count;
        var max = hand.maxCountDice;

        var countToDraw = max - currentCount;

        for (var i = 0; i < countToDraw; i++)
        {
            if (!hand.handStackController.DrawToHand())
            {
                break;
            }
        }
    }
}
