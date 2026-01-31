using System;
using System.Collections.Generic;
using System.Linq;
using BitterECS.Integration;
using UnityEngine;

public class Startup : EcsUnityRoot
{
    private GridConfig _gridConfig;
    private GenerationWorld _generationWorld;
    public static GameObject GridRaftParent;
    public static MonoGridPresenter GridWorld;

    protected override void Bootstrap()
    {
        _gridConfig = new Loader<GridConfig>(GridsPaths.GRID_WORLD).GetPrefab();
        _generationWorld = new Loader<GenerationWorld>(GridsPaths.GENERATION_GRID_WORLD).GetPrefab();
        GridWorld = new(_gridConfig);
        _generationWorld.Config(GridWorld);
        GridRaftParent = new GameObject("GridRaftParent");

        var playerPrefab = new Loader<PlayerProvider>(EntitiesPaths.PLAYER).GetInstance();
    }
}
