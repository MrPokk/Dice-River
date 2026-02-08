using BitterECS.Core;

public class HazardIsDestroySystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Medium;

    private EcsEvent _ecsEvent =
    new EcsEvent<EntitiesPresenter>()
        .Subscribe<IsDestroy>(added: OnDestroy);

    private static void OnDestroy(EcsEntity entity)
    {
        entity.Destroy();
    }
}
