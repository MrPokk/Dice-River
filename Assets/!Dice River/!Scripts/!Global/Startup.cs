using System;
using System.Collections.Generic;
using BitterECS.Core;
using BitterECS.Integration;
using InGame.Script.Component_Sound;
using UINotDependence.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Startup : EcsUnityRoot
{
    [ReadOnly] public CameraObject mainCamera;
    [ReadOnly] public SoundController soundManager;

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

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadConfigs()
    {
        _complicationSettings = new Loader<ComplicationSettings>(RiverObjectsPaths.COMPLICATION_SETTINGS).Prefab();
        GFlow.GState = new GState(_complicationSettings.difficultyStart, _complicationSettings.HazardSettings.hazardChance);
    }

    private void InitializeCamera()
    {
        mainCamera = new Loader<CameraObject>(PrefabObjectsPaths.CAMERA_OBJECT).New();
    }

    private void InitializeAudio()
    {
        soundManager = new Loader<SoundController>(PrefabObjectsPaths.AUDIO_OBJECT).New();
        SoundController.PlayMusicRandomPitch(SoundType.ForestMusicAmbience, volume: 0.1f, minPitch: 3, maxPitch: 5);
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
        var flowSystem = new GFlow();
        flowSystem.Initialize();

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
        UIFirstSwiping();
    }

    private static void UIFirstSwiping()
    {
        UIController.OpenPopup<UISwipingPopup>();
    }
}
