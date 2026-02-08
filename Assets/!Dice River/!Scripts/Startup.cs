using System;
using System.Collections.Generic;
using System.Linq;
using BitterECS.Core;
using BitterECS.Integration;
using UINotDependence.Core;
using UnityEngine;
using UnityEngine.XR;

public class Startup : EcsUnityRoot
{
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
        HandControllerDice = new Loader<HandControllerDice>(PrefabObjectsPaths.HAND_CONTROLLER).New();
        GridRaft.gridParent = new GameObject("GridRaftParent");

        _cameraObject = new Loader<CameraObject>(PrefabObjectsPaths.CAMERA_OBJECT).New();
        _gridConfigWorld = new Loader<GridConfig>(GridsPaths.GRID_WORLD).Prefab();
        _gridConfigRaftGeneration = new Loader<GridConfig>(GridsPaths.GRID_RAFT_GENERATION).Prefab();
        _gridConfigRaftInstallable = new Loader<GridConfig>(GridsPaths.GRID_RAFT_INSTALLABLE).Prefab();

        _riverGenerator = new Loader<RiverGenerator>(RiverObjectsPaths.RIVER_GENERATOR).New();
        _riverScroll = new Loader<RiverScrolling>(RiverObjectsPaths.RIVER_SCROLLER).New();
        GridWorld = new(_gridConfigWorld);
        GridRaft.monoGrid = new(_gridConfigRaftInstallable);
        _riverScroll.Initialize(_riverGenerator, GridWorld);
        var prefabsDice = new Loader<DiceProvider>(DicesPaths.BASE_DICE).Prefab();
        DiceRaftInitSystem.Initialize(_gridConfigRaftGeneration, prefabsDice);

        var playerPrefab = new Loader<PlayerProvider>(EntitiesPaths.PLAYER).New();
        _cameraObject.SetTarget(playerPrefab.transform);
    }

    protected override void PostBootstrap()
    {
        //var find = FindFirstObjectByType<UIh>();
        //find.Add(new Loader<DiceProvider>(DicesPaths.BASE_DICE).GetInstance());
        //find.Add(new Loader<DiceProvider>(DicesPaths.BASE_DICE).GetInstance());
        //find.Add(new Loader<DiceProvider>(DicesPaths.BASE_DICE).GetInstance());
    }
}

public class UIStartupSystem : IEcsInitSystem
{
    public Priority Priority => Priority.FIRST_TASK;

    public void Init()
    {
        new UIInit().Initialize();

        UIController.OpenScreen<UIHandScreen>();
        var handScreen = UIController.GetCurrentScreen as UIHandScreen;
        var test = new Loader<DiceProvider>(DicesPaths.BASE_DICE).Prefab();
        var icon = test.spriteIcon;

        handScreen.handDice.Add(icon.New());
        handScreen.handDice.Add(icon.New());
        handScreen.handDice.Add(icon.New());
    }
}
