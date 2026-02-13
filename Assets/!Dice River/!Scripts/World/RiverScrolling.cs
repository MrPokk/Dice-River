using System;
using System.Collections.Generic;
using UnityEngine;

public class RiverScrolling : MonoBehaviour
{
    private ComplicationSettings _complicationSettings;
    private RiverGenerator _generator;
    private MonoGridPresenter _presenter;
    private Transform _riverRoot;
    private Queue<GameObject> _rowLines = new();

    public event Action<float> OnDistanceChanged;

    private int _topRowIndex;
    private int _bottomRowIndex;
    private float _cellSizeZ;
    private float _movedDistance;
    private float _totalOffsetZ = 0f;

    private int _minCol;
    private int _maxCol;

    [ReadOnly] public float scrollSpeed = 1f;
    [SerializeField] private float _distanceMultiplier = 0.01f;
    public float TotalOffsetZ => _totalOffsetZ;

    public void Initialize(RiverGenerator generator, ComplicationSettings complication, MonoGridPresenter presenter)
    {
        _complicationSettings = complication;
        _presenter = presenter;
        _generator = generator;
        _riverRoot = new GameObject("RiverRoot").transform;

        scrollSpeed = complication.minSpeed;
        _cellSizeZ = _presenter.GetTotalCellSize().y;

        _minCol = _presenter.GetMinColumn();
        _maxCol = _presenter.GetMaxColumn();
        _bottomRowIndex = _presenter.GetMinRow();
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
            SpawnRowAt(r);
        }
    }

    private void Update()
    {
        if (_presenter == null || _riverRoot == null) return;

        var moveStep = scrollSpeed * Time.deltaTime;
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

    private void SpawnRowAt(int rowIndex)
    {
        var row = _generator.GenerateRow(_presenter, rowIndex, _riverRoot);
        row.transform.position = Vector3.back * _totalOffsetZ;
        _rowLines.Enqueue(row);
    }
}
