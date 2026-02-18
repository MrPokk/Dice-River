using System;
using System.Collections.Generic;
using UnityEngine;

public class RiverScrollingSystem : MonoBehaviour
{
    private ComplicationSettings _complicationSettings;
    private RiverGeneratorSystem _generator;
    private MonoGridPresenter _presenter;
    private Transform _riverRoot;
    private Queue<GameObject> _rowLines = new();

    public event Action<float> OnDistanceChanged;

    private int _topRowIndex;
    private int _bottomRowIndex;
    private int _startBottomIndex;
    private float _cellSizeZ;
    private float _movedDistance;
    [SerializeField, ReadOnly] private float _totalOffsetZ = 0f;

    private int _minCol;
    private int _maxCol;

    [ReadOnly] public float scrollSpeed = 0;
    [ReadOnly] public float speedFactor = 1f;
    [SerializeField] private float _distanceMultiplier = 0.2f;
    public float TotalScrollDistance => _totalOffsetZ;

    public void Initialize(RiverGeneratorSystem generator, ComplicationSettings complication, MonoGridPresenter presenter, List<EnvironmentToDestroy> startEnvironment = default)
    {
        _complicationSettings = complication;
        _presenter = presenter;
        _generator = generator;
        _riverRoot = new GameObject("RiverRoot").transform;

        scrollSpeed = 0;
        _cellSizeZ = _presenter.GetTotalCellSize().y;

        _minCol = _presenter.GetMinColumn();
        _maxCol = _presenter.GetMaxColumn();
        _bottomRowIndex = _presenter.GetMinRow();
        _startBottomIndex = _bottomRowIndex;
        _topRowIndex = _bottomRowIndex + _generator.SpawnDepth;

        for (var r = _bottomRowIndex; r <= _topRowIndex + 5; r++)
        {
            for (var x = _minCol; x <= _maxCol; x++)
            {
                _presenter.AddGridCell(new Vector2Int(x, r), null);
            }
        }

        for (var r = _bottomRowIndex; r < _topRowIndex; r++)
        {
            var rowObject = SpawnRowAt(r);

            if (startEnvironment != null)
            {
                foreach (var startEnv in startEnvironment)
                {
                    if (startEnv != null && startEnv.rowIndex == r)
                    {
                        startEnv.transform.SetParent(rowObject.transform, true);
                    }
                }
            }
        }
    }

    public void StartScrolling()
    {
        scrollSpeed = _complicationSettings.minSpeed;
    }

    private void Update()
    {
        if (_presenter == null || _riverRoot == null) return;

        var moveStep = scrollSpeed * speedFactor * Time.deltaTime;
        _totalOffsetZ += moveStep;

        OnDistanceChanged?.Invoke(_totalOffsetZ * _distanceMultiplier);
        _movedDistance += moveStep;

        foreach (var row in _rowLines)
        {
            if (row == null) continue;
            row.transform.position += Vector3.back * moveStep;
        }

        if (_movedDistance >= _cellSizeZ)
        {
            _movedDistance -= _cellSizeZ;
            ScrollProcess();
        }
    }

    private void ScrollProcess()
    {
        ClearBottomRow();
        SpawnTopRow();
    }

    private void ClearBottomRow()
    {
        if (_rowLines.Count == 0) return;

        var rowObject = _rowLines.Dequeue();
        Destroy(rowObject);

        for (var x = _minCol; x <= _maxCol; x++)
        {
            _presenter.SetValueInGrid(new Vector2Int(x, _bottomRowIndex), null);
        }

        _bottomRowIndex++;
    }

    private void SpawnTopRow()
    {
        for (var x = _minCol; x <= _maxCol; x++)
        {
            _presenter.AddGridCell(new Vector2Int(x, _topRowIndex), null);
        }

        SpawnRowAt(_topRowIndex);
        _topRowIndex++;
    }

    private GameObject SpawnRowAt(int rowIndex)
    {
        var row = rowIndex < _startBottomIndex + _complicationSettings.startGenerationToLine
            ? _generator.GenerateShoreOnly(_presenter, rowIndex, _riverRoot)
            : _generator.GenerateFullRow(_presenter, rowIndex, _riverRoot);

        row.transform.position = Vector3.back * _totalOffsetZ;
        _rowLines.Enqueue(row);
        return row;
    }
}
