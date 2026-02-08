using System;
using BitterECS.Core;

public class DiceIsDestroySystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;
    private EcsEvent _ecsEvent =
    new EcsEvent<DicePresenter>()
        .Subscribe<IsDestroy>(added: OnDiceDestroy);

    private static void OnDiceDestroy(EcsEntity entity)
    {
        var provider = entity.GetProvider<DiceProvider>();

        if (provider != null)
        {
            var disposeEntity = DiceInteractionSystem.Extraction(provider.transform.position);
            disposeEntity.Dispose();
        }
    }
}
