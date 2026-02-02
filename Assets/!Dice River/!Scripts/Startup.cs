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
    private GenerationWorld _generationWorld;
    public static MonoGridPresenter GridWorld;
    public static MonoGridPresenter GridRaft;

    protected override void Bootstrap()
    {
        _cameraObject = new Loader<CameraObject>(PrefabObjectsPaths.CAMERA_OBJECT).GetInstance();
        _gridConfigWorld = new Loader<GridConfig>(GridsPaths.GRID_WORLD).GetPrefab();
        _gridConfigRaft = new Loader<GridConfig>(GridsPaths.GRID_RAFT).GetPrefab();

        _generationWorld = new Loader<GenerationWorld>(GridsPaths.GENERATION_GRID_WORLD).GetPrefab();
        GridWorld = new(_gridConfigWorld);
        GridRaft = new(_gridConfigRaft);
        _generationWorld.Config(GridWorld);
        DiceRaftInitSystem.Config(GridRaft);

        var playerPrefab = new Loader<PlayerProvider>(EntitiesPaths.PLAYER).GetInstance();
        _cameraObject.SetTarget(playerPrefab.transform);
    }
}
