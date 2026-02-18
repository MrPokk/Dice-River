using System;
using System.Collections.Generic;
using BitterECS.Core;
using BitterECS.Integration;
using InGame.Script.Component_Sound;
using UINotDependence.Core;
using UnityEngine;
using UnityEngine.EventSystems;

public class Startup : EcsUnityRoot
{
    [ReadOnly] public CameraObject mainCamera;
    [ReadOnly] public SoundManager soundManager;

    public SpawnerPoint playerSpawner;
    public List<EnvironmentToDestroy> environmentToDestroy;
    public List<EnvironmentToEnable> environmentToEnables;

    private ComplicationSettings _complicationSettings;

    public static RiverGeneratorSystem RiverGenerator;
    public static RiverScrollingSystem RiverScroll;
    public static MonoGridPresenter GridWorld;
    public static (MonoGridPresenter monoGrid, GameObject gridParent) GridRaft;
    public static HandControllerDice HandControllerDice;
    public static HandStackControllerDice HandStackControllerDice;

    protected override void Bootstrap()
    {
        LoadConfigs();
        InitializeCamera();
        InitializeGrids();
        InitializeRiver();
        InitializeDiceSystem();
        InitializePlayer();
        InitializeAudio();
        InitializeGameplaySystems();
        UIInitialize();
    }

    public void Restart()
    {
        Bootstrap();
    }

    private void LoadConfigs()
    {
        _complicationSettings = new Loader<ComplicationSettings>(RiverObjectsPaths.COMPLICATION_SETTINGS).Prefab();
    }

    private void InitializeCamera()
    {
        mainCamera = new Loader<CameraObject>(PrefabObjectsPaths.CAMERA_OBJECT).New();
    }

    private void InitializeAudio()
    {
        soundManager = new Loader<SoundManager>(PrefabObjectsPaths.AUDIO_OBJECT).New();
        SoundManager.PlayMusicRandomPitch(SoundType.ForestMusicAmbience, volume: 0.1f, minPitch: 3, maxPitch: 5);
        SoundManager.PlayMusic(SoundType.ForestMusic, true, 0.8f, 0.8f);

    }

    private void InitializeGrids()
    {
        GridRaft.gridParent = new GameObject("GridRaftParent");

        GridWorld = new MonoGridPresenter(new Loader<GridConfig>(GridsPaths.GRID_WORLD).Prefab());
        GridRaft.monoGrid = new MonoGridPresenter(new Loader<GridConfig>(GridsPaths.GRID_RAFT_INSTALLABLE).Prefab());
    }

    private void InitializeRiver()
    {
        RiverGenerator = new Loader<RiverGeneratorSystem>(RiverObjectsPaths.RIVER_GENERATOR).New();
        RiverScroll = new Loader<RiverScrollingSystem>(RiverObjectsPaths.RIVER_SCROLLER).New();
        RiverScroll.Initialize(RiverGenerator, _complicationSettings, GridWorld, environmentToDestroy);
        foreach (var environment in environmentToEnables)
        {
            environment.Activate();
        }
    }

    private void InitializeGameplaySystems()
    {
        var flowSystem = new StartupGameplay();
        flowSystem.Initialize(_complicationSettings);
        EcsSystems.AddSystem(flowSystem);

        var complicationSystem = new ComplicationGameplaySystem(RiverScroll, _complicationSettings);
        EcsSystems.AddSystem(complicationSystem);
    }

    private void InitializeDiceSystem()
    {
        var prefabsDice = new Loader<DiceProvider>(DicesPaths.BASE_DICE).Prefab();
        var raftGenCfg = new Loader<GridConfig>(GridsPaths.GRID_RAFT_GENERATION).Prefab();
        DiceRaftInitSystem.Initialize(raftGenCfg, prefabsDice);
    }

    private void InitializePlayer()
    {
        var playerPrefab = new Loader<PlayerProvider>(EntitiesPaths.PLAYER).New(playerSpawner.transform.position, Quaternion.identity);
        mainCamera.SetTarget(playerPrefab.transform);
    }

    private void UIInitialize()
    {
        UIInit.Initialize();
        new Loader<EventSystem>(SettingsPaths.EVENT_SYSTEM).New();
        UIController.OpenScreen<UIToStartFloating>();
    }

    public static void StartGameplay()
    {
        HandControllerDice = new Loader<HandControllerDice>(HandPaths.HAND_CONTROLLER).New();
        HandStackControllerDice = new Loader<HandStackControllerDice>(HandPaths.HAND_STACK_CONTROLLER).New();

        HandStackControllerDice.Initialize(HandControllerDice);
        HandControllerDice.Initialize(HandStackControllerDice);

        RiverScroll.StartScrolling();
        UIController.OpenScreen<UIPlayerScreen>()
        .Bind(HandControllerDice, HandStackControllerDice, RiverScroll);
    }
}
