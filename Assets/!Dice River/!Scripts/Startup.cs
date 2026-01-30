using System;
using System.Collections.Generic;
using System.Linq;
using BitterECS.Integration;
using UnityEngine;

public class Startup : EcsUnityRoot
{
    private GridConfig _gridConfig;
    private static GameObject s_gridRaftParent;
    public static MonoGridPresenter GridRaft;

    private float _seed;
    private int _minX;
    private int _maxX;

    // Настройки русла (подбери под размер своей сетки)
    private const int MinRiverWidth = 12;
    private const float NoiseScale = 0.12f;
    private const float MeanderAmount = 4f;

    protected override void Bootstrap()
    {
        _gridConfig = new Loader<GridConfig>(GridsPaths.GRID_WORLD).GetPrefab();
        GridRaft = new(_gridConfig);
        s_gridRaftParent = new GameObject("GridRaftParent");

        // 1. Получаем реальные границы из конфига (те самые [-9, 9] со скриншота)
        var nodes = GridRaft.GetGridNodes();
        if (nodes != null && nodes.Count > 0)
        {
            _minX = nodes.Keys.Min(k => k.x);
            _maxX = nodes.Keys.Max(k => k.x);
        }
        else
        {
            _minX = -10; // Резервные значения
            _maxX = 10;
        }

        _seed = UnityEngine.Random.value * 10000f;

        var playerPrefab = new Loader<PlayerProvider>(EntitiesPaths.PLAYER).GetPrefab();
        Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        // Генерируем начальную зону (например, от -10 до 20 по Y)
        for (int i = -10; i < 20; i++)
        {
            GenerateRiverRow(i);
        }
    }

    public static void SpawnDiceRaft(Vector2Int index, ProviderEcs prefab, out ProviderEcs instantiateObject)
    {
        // ВАЖНО: Если ячейки нет в словаре, добавляем её, иначе InitializeGameObject вернет false
        if (!GridRaft.IsWithinGrid(index))
        {
            GridRaft.AddGridCell(index);
        }

        GridRaft.InitializeGameObject(index, prefab, out instantiateObject, s_gridRaftParent.transform);
    }

    private void GenerateRiverRow(int yIndex)
    {
        // Рассчитываем положение центра реки
        float noise = Mathf.PerlinNoise(_seed, yIndex * NoiseScale) * 2 - 1;
        int centerOffset = Mathf.RoundToInt(noise * MeanderAmount);

        // Определяем границы воды (прохода)
        int halfWidth = MinRiverWidth / 2;
        int leftWaterBorder = centerOffset - halfWidth;
        int rightWaterBorder = centerOffset + halfWidth;

        // Проходим по ВСЕМ колонкам от края до края сетки
        for (int x = _minX; x <= _maxX; x++)
        {
            // Если X находится ВНЕ границ воды — это берег
            if (x <= leftWaterBorder || x >= rightWaterBorder)
            {
                var shorePrefab = new Loader<ProviderEcs>(DicesPaths.GENERAL_DICE).GetPrefab();
                SpawnDiceRaft(new Vector2Int(x, yIndex), shorePrefab, out _);
            }
        }
    }
}
