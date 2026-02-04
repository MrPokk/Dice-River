using System;
using System.Collections.Generic;
using System.Linq;
using BitterECS.Integration;
using UnityEngine;

public class Startup : EcsUnityRoot
{
    private CameraObject _cameraObject;
    private GridConfig _gridConfigWorld;
    private GridConfig _gridConfigRaftGeneration;
    private GridConfig _gridConfigRaftInstallable;
    private RiverGenerator _riverGenerator;
    private RiverScroll _riverScroll;
    public static MonoGridPresenter GridWorld;
    public static (MonoGridPresenter monoGrid, GameObject gridParent) GridRaft;
    public static GameObject GridRaftParent;

    protected override void Bootstrap()
    {
        GridRaft.gridParent = new GameObject("GridRaftParent");

        _cameraObject = new Loader<CameraObject>(PrefabObjectsPaths.CAMERA_OBJECT).GetInstance();
        _gridConfigWorld = new Loader<GridConfig>(GridsPaths.GRID_WORLD).GetPrefab();
        _gridConfigRaftGeneration = new Loader<GridConfig>(GridsPaths.GRID_RAFT_GENERATION).GetPrefab();
        _gridConfigRaftInstallable = new Loader<GridConfig>(GridsPaths.GRID_RAFT_INSTALLABLE).GetPrefab();

        _riverGenerator = new Loader<RiverGenerator>(GridsPaths.RIVER_GENERATOR).GetInstance();
        _riverScroll = new Loader<RiverScroll>(GridsPaths.RIVER_SCROLLER).GetInstance();
        GridWorld = new(_gridConfigWorld);
        GridRaft.monoGrid = new(_gridConfigRaftInstallable);
        _riverScroll.Initialize(_riverGenerator, GridWorld);
        var prefabsDice = new Loader<DiceProvider>(DicesPaths.TEST_DICE).GetPrefab();
        DiceRaftInitSystem.Initialize(_gridConfigRaftGeneration, prefabsDice);

        var playerPrefab = new Loader<PlayerProvider>(EntitiesPaths.PLAYER).GetInstance();
        _cameraObject.SetTarget(playerPrefab.transform);
    }
}
