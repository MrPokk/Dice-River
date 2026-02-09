using System.Linq;
using BitterECS.Core;
using UnityEngine;

public class HandControllerDice : HandController<EcsEntity, UIProvider>
{
    [Header("Setting Controller Dice")]
    public float timeRefreshSecond;

    public void Initialize()
    {
        var p = new Loader<DiceProvider>(DicesPaths.BASE_DICE).Prefab();
        for (int i = 0; i < 3; i++) Add(p.NewEntity(), p.spriteIcon.Prefab());
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
