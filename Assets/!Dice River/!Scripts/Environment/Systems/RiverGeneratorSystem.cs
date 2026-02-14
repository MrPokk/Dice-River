using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;
using Random = UnityEngine.Random;

public class RiverGeneratorSystem : MonoBehaviour
{
    private struct RowData
    {
        public State prevState;
        public int minCol, maxCol;
        public GameObject rowLine;
        public float lBound, rBound;
        public Vector2Int cLShore, cRShore;
    }

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

    public GameObject GenerateShoreOnly(MonoGridPresenter presenter, int row, Transform parent)
    {
        var rowData = InitRow(presenter, row, parent);

        SpawnDecoration(presenter, row, rowData.minCol, rowData.maxCol, rowData.rowLine, rowData.lBound, rowData.rBound, rowData.cLShore, rowData.cRShore);

        state = rowData.prevState;
        return rowData.rowLine;
    }

    public GameObject GenerateHazardRow(MonoGridPresenter presenter, int row, Transform parent)
    {
        var rowData = InitRow(presenter, row, parent);

        SpawnDecoration(presenter, row, rowData.minCol, rowData.maxCol, rowData.rowLine, rowData.lBound, rowData.rBound, rowData.cLShore, rowData.cRShore);
        SpawnHazard(presenter, row, rowData.lBound, rowData.rBound, rowData.rowLine.transform);

        state = rowData.prevState;
        return rowData.rowLine;
    }

    public GameObject GeneratePickupRow(MonoGridPresenter presenter, int row, Transform parent)
    {
        var rowData = InitRow(presenter, row, parent);

        SpawnDecoration(presenter, row, rowData.minCol, rowData.maxCol, rowData.rowLine, rowData.lBound, rowData.rBound, rowData.cLShore, rowData.cRShore);
        SpawnPickup(presenter, row, rowData.lBound, rowData.rBound, rowData.rowLine.transform);

        state = rowData.prevState;
        return rowData.rowLine;
    }

    public GameObject GenerateFullRow(MonoGridPresenter presenter, int row, Transform parent)
    {
        var rowData = InitRow(presenter, row, parent);

        SpawnDecoration(presenter, row, rowData.minCol, rowData.maxCol, rowData.rowLine, rowData.lBound, rowData.rBound, rowData.cLShore, rowData.cRShore);
        SpawnHazard(presenter, row, rowData.lBound, rowData.rBound, rowData.rowLine.transform);
        SpawnPickup(presenter, row, rowData.lBound, rowData.rBound, rowData.rowLine.transform);

        state = rowData.prevState;
        return rowData.rowLine;
    }

    private RowData InitRow(MonoGridPresenter presenter, int row, Transform parent)
    {
        GetRowParams(
            presenter,
            row,
            parent,
            out var ps,
            out var min,
            out var max,
            out var line,
            out var lb,
            out var rb,
            out var cls,
            out var crs);

        return new RowData
        {
            prevState = ps,
            minCol = min,
            maxCol = max,
            rowLine = line,
            lBound = lb,
            rBound = rb,
            cLShore = cls,
            cRShore = crs
        };
    }

    private void GetRowParams(
        MonoGridPresenter presenter,
        int row,
        Transform parent,
        out Random.State previousState,
        out int indexColumnMin,
        out int indexColumnMax,
        out GameObject rowLine,
        out float leftBound,
        out float rightBound,
        out Vector2Int centerLeftShoreNode,
        out Vector2Int centerRightShoreNode)
    {
        previousState = state;
        var rowSeed = (_seed * 73856093) ^ (row * 19349663);
        InitState(rowSeed);
        _usedXInRow.Clear();

        indexColumnMin = presenter.GetMinColumn();
        indexColumnMax = presenter.GetMaxColumn();
        var gridCenter = (indexColumnMin + indexColumnMax) / 2f;

        rowLine = new GameObject($"Line_Row_Y{row}");
        rowLine.transform.SetParent(parent);

        var num = row + _zOffset;
        var seedOffset = _seed * 100f;

        var pathShift = Mathf.Floor((Mathf.PerlinNoise(_pathZScale * num + seedOffset, 0.2f + seedOffset) - 0.5f) * _pathNoise);
        var dynamicWidth = Mathf.Floor(_width + Mathf.PerlinNoise(0.245f + seedOffset, _widthZScale * num + seedOffset) * _widthNoise);

        leftBound = gridCenter + pathShift - dynamicWidth;
        rightBound = gridCenter + pathShift + dynamicWidth;
        centerLeftShoreNode = new Vector2Int(Mathf.RoundToInt((indexColumnMin + leftBound) / 2f), row);
        centerRightShoreNode = new Vector2Int(Mathf.RoundToInt((rightBound + indexColumnMax) / 2f), row);
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
            if (value < hazardChance)
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
            if (value < pickupChance)
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
            if (x > leftBound && x < rightBound) continue;
            if (_usedXInRow.Contains(x)) continue;

            var prefab = _shoreSettings.decorationSettings.GetRandomTree();
            presenter.OneFrameInitializeGameObject(new Vector2Int(x, row), prefab, out _, rowLine.transform);
            _usedXInRow.Add(x);
        }
    }

    private void SpawnDecorationShadow(MonoGridPresenter presenter, Vector2Int node, Transform parent)
    {
        var decorationPrefab = _shoreSettings.decorationSettings.GetRandomShadow();
        presenter.OneFrameInitializeGameObject(node, decorationPrefab, out _, parent);
    }
}
