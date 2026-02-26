using BitterECS.Core;
using BitterECS.Integration.Unity;
using InGame.Script.Component_Sound;
using UINotDependence.Core;

public class GFlow
{
    public static GState GState;
    private EcsEvent _ecsEvent;

    public void Initialize()
    {
        _ecsEvent = new EcsEvent<EntitiesPresenter>()
            .SubscribeWhereEntity<IsColliderHit>(
                e => e.Has<InputComponent>(),
                added: OnPlayerFirstContactToStartGameplay
            );
    }

    private void OnPlayerFirstContactToStartGameplay(EcsEntity entity)
    {
        if (!PlayerProvider.IsPlayerContact(entity, out _, out _, out _)) return;

        GState.isFirstStart = true;
        Startup.HandControllerDice = new Loader<HandControllerDice>(HandPaths.HAND_CONTROLLER).New();
        Startup.HandStackControllerDice = new Loader<HandStackControllerDice>(HandPaths.HAND_STACK_CONTROLLER).New();

        Startup.HandStackControllerDice.Initialize(Startup.HandControllerDice);
        Startup.HandControllerDice.Initialize(Startup.HandStackControllerDice);

        Startup.RiverScroll.StartScrolling();
        SoundController.PlayMusic(SoundType.ForestMusic, true, 0.8f, 0.8f);
        UIController.OpenScreen<UIPlayerScreen>()
        .Bind(Startup.HandControllerDice, Startup.HandStackControllerDice, Startup.RiverScroll);

        EcsSystems.Run<IStartToGameplay>(s => s.ToStart());
        _ecsEvent.Dispose();
    }
}
