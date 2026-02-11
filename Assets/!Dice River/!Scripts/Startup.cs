using BitterECS.Core;
using BitterECS.Integration;
using UINotDependence.Core;
using UnityEngine;

public class Startup : EcsUnityRoot
{
    public SpawnerPoint playerSpawner;

    private CameraObject _cameraObject;
    private GridConfig _gridConfigWorld;
    private GridConfig _gridConfigRaftGeneration;
    private GridConfig _gridConfigRaftInstallable;
    private RiverGenerator _riverGenerator;
    private RiverScrolling _riverScroll;
    public static MonoGridPresenter GridWorld;
    public static (MonoGridPresenter monoGrid, GameObject gridParent) GridRaft;
    public static GameObject GridRaftParent;
    public static HandControllerDice HandControllerDice;

    protected override void Bootstrap()
    {
        InitializeHandController();
        InitializeCamera();
        InitializeGrids();
        InitializeRiver();
        InitializeDiceSystem();
        InitializePlayer();
        UIInitialize();
    }

    private void InitializeHandController()
    {
        HandControllerDice = new Loader<HandControllerDice>(PrefabObjectsPaths.HAND_CONTROLLER).New();
        HandControllerDice.Initialize();
    }

    private void InitializeCamera()
    {
        _cameraObject = new Loader<CameraObject>(PrefabObjectsPaths.CAMERA_OBJECT).New();
    }

    private void InitializeGrids()
    {
        GridRaft.gridParent = new GameObject("GridRaftParent");

        _gridConfigWorld = new Loader<GridConfig>(GridsPaths.GRID_WORLD).Prefab();
        _gridConfigRaftGeneration = new Loader<GridConfig>(GridsPaths.GRID_RAFT_GENERATION).Prefab();
        _gridConfigRaftInstallable = new Loader<GridConfig>(GridsPaths.GRID_RAFT_INSTALLABLE).Prefab();

        GridWorld = new MonoGridPresenter(_gridConfigWorld);
        GridRaft.monoGrid = new MonoGridPresenter(_gridConfigRaftInstallable);
    }

    private void InitializeRiver()
    {
        _riverGenerator = new Loader<RiverGenerator>(RiverObjectsPaths.RIVER_GENERATOR).New();
        _riverScroll = new Loader<RiverScrolling>(RiverObjectsPaths.RIVER_SCROLLER).New();
        _riverScroll.Initialize(_riverGenerator, GridWorld);
    }

    private void InitializeDiceSystem()
    {
        var prefabsDice = new Loader<DiceProvider>(DicesPaths.BASE_DICE).Prefab();
        DiceRaftInitSystem.Initialize(_gridConfigRaftGeneration, prefabsDice);
    }

    private void InitializePlayer()
    {
        var playerPrefab = new Loader<PlayerProvider>(EntitiesPaths.PLAYER).New(playerSpawner.transform.position, Quaternion.identity);
        _cameraObject.SetTarget(playerPrefab.transform);
    }

    private void UIInitialize()
    {
        UIInit.Initialize();

        UIController.OpenScreen<UIPlayerScreen>();
        var playerScreen = (UIPlayerScreen)UIController.GetCurrentScreen;

        playerScreen.Bind(HandControllerDice);
    }
}
