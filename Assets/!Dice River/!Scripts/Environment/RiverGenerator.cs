using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RiverGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _seed;
    [SerializeField] private int _spawnDepth = 40;
    [SerializeField] private int _width = 6;
    [SerializeField] private float _zOffset = 6f;
    [SerializeField] private float _pathNoise = 6f;
    [SerializeField] private float _pathZScale = 0.1f;
    [SerializeField] private float _widthNoise = 5f;
    [SerializeField] private float _widthZScale = 0.1f;

    [Header("Assets")]
    [SerializeField] private RiverSettings _shoreSettings;

    private readonly HashSet<int> _usedXInRow = new();

    public int SpawnDepth => _spawnDepth;

    public GameObject GenerateRow(MonoGridPresenter presenter, int row, Transform parent)
    {
        var previousState = Random.state;
        var rowSeed = (_seed * 73856093) ^ (row * 19349663);
        Random.InitState(rowSeed);

        _usedXInRow.Clear();

        var indexColumnMin = presenter.GetMinColumn();
        var indexColumnMax = presenter.GetMaxColumn();
        var gridCenter = (indexColumnMin + indexColumnMax) / 2f;

        var rowLine = new GameObject($"Line_Row_Y{row}");
        rowLine.transform.SetParent(parent);

        var num = row + _zOffset;
        var seedOffset = _seed * 100f;

        var pathShift = Mathf.Floor((Mathf.PerlinNoise(_pathZScale * num + seedOffset, 0.2f + seedOffset) - 0.5f) * _pathNoise);
        var dynamicWidth = Mathf.Floor(_width + Mathf.PerlinNoise(0.245f + seedOffset, _widthZScale * num + seedOffset) * _widthNoise);

        var leftBound = gridCenter + pathShift - dynamicWidth;
        var rightBound = gridCenter + pathShift + dynamicWidth;

        var centerLeftShoreNode = new Vector2Int(Mathf.RoundToInt((indexColumnMin + leftBound) / 2f), row);
        var centerRightShoreNode = new Vector2Int(Mathf.RoundToInt((rightBound + indexColumnMax) / 2f), row);

        SpawnDecoration(
            presenter,
            row,
            indexColumnMin,
            indexColumnMax,
            rowLine,
            leftBound,
            rightBound,
            centerLeftShoreNode,
            centerRightShoreNode);

        SpawnHazard(
            presenter,
            row,
            leftBound,
            rightBound,
            rowLine.transform);

        SpawnPickup(
            presenter,
            row,
            leftBound,
            rightBound,
            rowLine.transform);

        Random.state = previousState;

        return rowLine;
    }

    private void SpawnHazard(MonoGridPresenter presenter, int row, float leftBound, float rightBound, Transform parent)
    {
        var waterStart = Mathf.CeilToInt(leftBound);
        var waterEnd = Mathf.FloorToInt(rightBound);
        var hazardSettings = _shoreSettings.hazardSettings;
        var hazardChance = _shoreSettings.hazardSettings.hazardChance;

        for (var x = waterStart; x <= waterEnd; x++)
        {
            if (_usedXInRow.Contains(x)) continue;

            if (Random.value < hazardChance)
            {
                var node = new Vector2Int(x, row);
                var hazardPrefab = hazardSettings.GetRandom();

                if (hazardPrefab != null)
                {
                    presenter.OneFrameInitializeGameObject(node, hazardPrefab, out _, parent);
                    _usedXInRow.Add(x);
                }
            }
        }
    }

    private void SpawnPickup(MonoGridPresenter presenter, int row, float leftBound, float rightBound, Transform parent)
    {
        var waterStart = Mathf.CeilToInt(leftBound);
        var waterEnd = Mathf.FloorToInt(rightBound);
        var pickupSettings = _shoreSettings.pickupSettings;
        var pickupChance = _shoreSettings.pickupSettings.pickupChance;

        for (var x = waterStart; x <= waterEnd; x++)
        {
            if (_usedXInRow.Contains(x)) continue;

            if (Random.value < pickupChance)
            {
                var node = new Vector2Int(x, row);
                var hazardPrefab = pickupSettings.GetRandom();

                if (hazardPrefab != null)
                {
                    presenter.OneFrameInitializeGameObject(node, hazardPrefab, out _, parent);
                    _usedXInRow.Add(x);
                }
            }
        }
    }

    private void SpawnDecoration(
        MonoGridPresenter presenter,
        int row,
        int indexColumnMin,
        int indexColumnMax,
        GameObject rowLine,
        float leftBound,
        float rightBound,
        Vector2Int centerLeftShoreNode,
        Vector2Int centerRightShoreNode)
    {
        SpawnDecorationShadow(presenter, centerLeftShoreNode, rowLine.transform);
        _usedXInRow.Add(centerLeftShoreNode.x);

        SpawnDecorationShadow(presenter, centerRightShoreNode, rowLine.transform);
        _usedXInRow.Add(centerRightShoreNode.x);

        for (var x = indexColumnMin; x <= indexColumnMax; x++)
        {
            var node = new Vector2Int(x, row);
            if (x > leftBound && x < rightBound)
            {
                continue;
            }

            if (_usedXInRow.Contains(x)) continue;

            var prefab = _shoreSettings.decorationSettings.GetRandomTree();
            presenter.OneFrameInitializeGameObject(node, prefab, out _, rowLine.transform);
            _usedXInRow.Add(x);
        }
    }

    private void SpawnDecorationShadow(MonoGridPresenter presenter, Vector2Int node, Transform parent)
    {
        var decorationPrefab = _shoreSettings.decorationSettings.GetRandomShadow();
        presenter.OneFrameInitializeGameObject(node, decorationPrefab, out _, parent);
    }
}
