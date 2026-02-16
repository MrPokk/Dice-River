using BitterECS.Core;
using BitterECS.Integration;
using DG.Tweening;

public class DiceShakingSystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Low;

    private readonly EcsEvent _ecsEventAdding =
        new EcsEvent<DicePresenter>()
            .SubscribeAny<IsTargetingEvent, IsActivatingEvent, IsPlacingEvent>(
                added: OnShaking
            );

    private static void OnShaking(EcsEntity entity)
    {
        var providerEcs = entity.GetProvider<DiceProvider>();
        providerEcs.transform.DOShakePosition(0.5f, 0.03f, 10, 30, false, true)
            .SetEase(Ease.InOutSine)
            .Play();
    }
}
