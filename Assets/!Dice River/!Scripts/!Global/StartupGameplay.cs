using BitterECS.Core;

public class StartupGameplay : IEcsSystem
{
    public Priority Priority => Priority.High;
    public static GState GState;
    private EcsEvent _ecsEvent;

    public void Initialize(ComplicationSettings settings)
    {
        GState = new GState(settings.difficultyStart);
        _ecsEvent = new EcsEvent<EntitiesPresenter>()
            .SubscribeWhereEntity<IsColliderHit>(
                e => e.Has<InputComponent>(),
                added: OnPlayerFirstContact
            );
    }

    private void OnPlayerFirstContact(EcsEntity entity)
    {
        if (!PlayerProvider.IsPlayerContact(entity, out _, out _, out _)) return;

        GState.isFirstStart = true;
        Startup.StartGameplay();
        _ecsEvent.Dispose();
    }
}
