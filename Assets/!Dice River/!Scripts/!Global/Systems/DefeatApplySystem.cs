using System;
using BitterECS.Core;
using UINotDependence.Core;

public class DefeatApplySystem : IEcsAutoImplement
{
    public Priority Priority => Priority.High;

    private EcsEvent _ecsEvent = new EcsEvent<EntitiesPresenter>()
        .SubscribeWhereEntity<IsHealthChanging>(e => e.Has<InputComponent>(), added: OnDefeat);

    private static void OnDefeat(EcsEntity entity)
    {
        ref var healthComp = ref entity.Get<HealthComponent>();
        if (healthComp.currentHealth <= 0)
        {
            UIController.OpenScreen<UIDefeatScreen>();
            Startup.RiverScroll.scrollSpeed = 0;
            entity.Destroy();
        }
    }
}
