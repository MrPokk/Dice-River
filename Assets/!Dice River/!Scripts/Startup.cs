using System;
using System.Collections.Generic;
using System.Linq;
using BitterECS.Integration;
using UIFeture.Core;
using UnityEngine;

public class Startup : EcsUnityRoot
{
    private UIEntryPoint _uiEntryPoint;
    private CameraObject _cameraObject;
    private GridConfig _gridConfigWorld;
    private GridConfig _gridConfigRaftGeneration;
    private GridConfig _gridConfigRaftInstallable;
    private RiverGenerator _riverGenerator;
    private RiverScrolling _riverScroll;
    public static MonoGridPresenter GridWorld;
    public static (MonoGridPresenter monoGrid, GameObject gridParent) GridRaft;
    public static GameObject GridRaftParent;


    protected override void Bootstrap()
    {
        _uiEntryPoint = new();
        _uiEntryPoint.Start();
        GridRaft.gridParent = new GameObject("GridRaftParent");

        _cameraObject = new Loader<CameraObject>(PrefabObjectsPaths.CAMERA_OBJECT).GetInstance();
        _gridConfigWorld = new Loader<GridConfig>(GridsPaths.GRID_WORLD).GetPrefab();
        _gridConfigRaftGeneration = new Loader<GridConfig>(GridsPaths.GRID_RAFT_GENERATION).GetPrefab();
        _gridConfigRaftInstallable = new Loader<GridConfig>(GridsPaths.GRID_RAFT_INSTALLABLE).GetPrefab();

        _riverGenerator = new Loader<RiverGenerator>(RiverObjectsPaths.RIVER_GENERATOR).GetInstance();
        _riverScroll = new Loader<RiverScrolling>(RiverObjectsPaths.RIVER_SCROLLER).GetInstance();
        GridWorld = new(_gridConfigWorld);
        GridRaft.monoGrid = new(_gridConfigRaftInstallable);
        _riverScroll.Initialize(_riverGenerator, GridWorld);
        var prefabsDice = new Loader<DiceProvider>(DicesPaths.BASE_DICE).GetPrefab();
        DiceRaftInitSystem.Initialize(_gridConfigRaftGeneration, prefabsDice);

        var playerPrefab = new Loader<PlayerProvider>(EntitiesPaths.PLAYER).GetInstance();
        _cameraObject.SetTarget(playerPrefab.transform);
    }

    protected override void PostBootstrap()
    {
        var handDice = FindFirstObjectByType<HandDice>();
        handDice.Add(new Loader<DiceProvider>(DicesPaths.BASE_DICE).GetInstance());
        handDice.Add(new Loader<DiceProvider>(DicesPaths.BASE_DICE).GetInstance());
        handDice.Add(new Loader<DiceProvider>(DicesPaths.BASE_DICE).GetInstance());
    }
}
