using System.Linq;
using BitterECS.Core;
using UnityEngine;

public class HandControllerDice : HandController<EcsEntity, UIProvider>
{
    [Header("Setting Controller Dice")]
    public float timeRefreshSecond;
    public uint maxCountDice;

    public override void Initialize(HandStackController<EcsEntity, UIProvider> handStackController)
    {
        base.Initialize(handStackController);
        for (var i = 0; i < maxCountDice; i++)
        {
            handStackController.DrawToHand();
        }
    }

    public override bool ExtractToFirst(out EcsEntity val)
    {
        var result = base.ExtractToFirst(out val);
        if (result)
        {
            EcsSystems.Run<IHandSucceedExtraction>(s => s.ResultSucceedExtraction(this));
            if (Items.Count == 0) EcsSystems.Run<IHandResultInExtractEnded>(s => s.ResultInExtractEnded(this));
        }
        else EcsSystems.Run<IHandFailExtraction>(s => s.ResultFailExtraction(this));

        return result;
    }

    public override bool Add(EcsEntity data, UIProvider view)
    {
        if (Items.Count >= maxCountDice)
        {
            EcsSystems.Run<IHandFailAdd>(s => s.ResultFailAdd(this));
            return false;
        }

        var result = base.Add(data, view);
        if (result) EcsSystems.Run<IHandSucceedAdd>(s => s.ResultSucceedAdd(this));
        else EcsSystems.Run<IHandFailAdd>(s => s.ResultFailAdd(this));
        return result;
    }

    public override bool Remove(EcsEntity data)
    {
        var result = base.Remove(data);
        if (result)
        {
            EcsSystems.Run<IHandSucceedRemove>(s => s.ResultSucceedRemove(this));
            if (Items.Count == 0) EcsSystems.Run<IHandResultInRemoveEnded>(s => s.ResultInRemoveEnded(this));
        }
        else EcsSystems.Run<IHandFailRemove>(s => s.ResultFailRemove(this));

        return result;
    }
}
