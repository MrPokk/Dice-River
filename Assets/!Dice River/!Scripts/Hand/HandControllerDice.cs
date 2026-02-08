using BitterECS.Core;

public class HandControllerDice : HandController<EcsEntity, UIProvider>
{

    public void Initialize()
    {
        var prefabDice = new Loader<DiceProvider>(DicesPaths.BASE_DICE).Prefab();
        Add(prefabDice.NewEntity(), prefabDice.spriteIcon.Prefab());
        Add(prefabDice.NewEntity(), prefabDice.spriteIcon.Prefab());
        Add(prefabDice.NewEntity(), prefabDice.spriteIcon.Prefab());
    }

    public override bool ExtractToFirst(out EcsEntity value)
    {
        var isExtract = base.ExtractToFirst(out value);
        if (isExtract)
        {
            EcsSystems.Run<IHandSucceedExtraction>(s => s.ResultSucceedExtraction());
        }
        else
        {
            EcsSystems.Run<IHandFailExtraction>(s => s.ResultFailExtraction());
        }
        return isExtract;
    }

    public override bool Add(EcsEntity data, UIProvider viewPrefab)
    {
        var isAdding = base.Add(data, viewPrefab);
        if (isAdding)
        {
            EcsSystems.Run<IHandSucceedAdd>(s => s.ResultSucceedAdd());
        }
        else
        {
            EcsSystems.Run<IHandFailAdd>(s => s.ResultFailAdd());
        }
        return isAdding;
    }

    public override bool Remove(EcsEntity data)
    {
        var isRemoved = base.Remove(data);
        if (isRemoved)
        {
            EcsSystems.Run<IHandSucceedRemove>(s => s.ResultSucceedRemove());
        }
        else
        {
            EcsSystems.Run<IHandFailRemove>(s => s.ResultFailRemove());
        }
        return isRemoved;
    }
}
