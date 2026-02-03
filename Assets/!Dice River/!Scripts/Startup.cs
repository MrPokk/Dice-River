using System;
using System.Collections.Generic;
using System.Linq;
using BitterECS.Integration;
using UnityEngine;

public class Startup : EcsUnityRoot
{
    private CameraObject _cameraObject;
    private GridConfig _gridConfigWorld;
    private GridConfig _gridConfigRaft;
    private RiverGenerator _riverGenerator;
    private RiverScroll _riverScroll;
    public static MonoGridPresenter GridWorld;
    public static MonoGridPresenter GridRaft;

    protected override void Bootstrap()
    {
        _cameraObject = new Loader<CameraObject>(PrefabObjectsPaths.CAMERA_OBJECT).GetInstance();
        _gridConfigWorld = new Loader<GridConfig>(GridsPaths.GRID_WORLD).GetPrefab();
        _gridConfigRaft = new Loader<GridConfig>(GridsPaths.GRID_RAFT).GetPrefab();

        _riverGenerator = new Loader<RiverGenerator>(GridsPaths.RIVER_GENERATOR).GetInstance();
        _riverScroll = new Loader<RiverScroll>(GridsPaths.RIVER_SCROLLER).GetInstance();
        GridWorld = new(_gridConfigWorld);
        GridRaft = new(_gridConfigRaft);
        _riverScroll.Initialize(_riverGenerator, GridWorld);
        DiceRaftInitSystem.Initialize(GridRaft);

        var playerPrefab = new Loader<PlayerProvider>(EntitiesPaths.PLAYER).GetInstance();
        _cameraObject.SetTarget(playerPrefab.transform);
    }
}
