using System.Collections.Generic;
using UnityEngine;

public class RiverScroll : MonoBehaviour
{
    [SerializeField] private RiverGenerator _generator;
    [SerializeField] private float _scrollSpeed = 5f;

    private MonoGridPresenter _presenter;
    private Transform _riverRoot;
    private Queue<GameObject> _rowLines = new();

    private int _topRowIndex;
    private int _bottomRowIndex;
    private float _cellSizeZ;
    private float _movedDistance;
    private float _totalOffsetZ = 0f;

    public void Initialize(RiverGenerator generator, MonoGridPresenter presenter)
    {
        _presenter = presenter;
        _generator = generator;

        _riverRoot = new GameObject("RiverRoot").transform;

        _cellSizeZ = _presenter.GetTotalCellSize().y;
        _bottomRowIndex = _presenter.GetMinRow();
        _topRowIndex = _bottomRowIndex + _generator.SpawnDepth;

        for (int r = _bottomRowIndex; r < _topRowIndex; r++)
        {
            SpawnRowAt(r);
        }
    }

    private void Update()
    {
        if (_presenter == null || _riverRoot == null) return;

        float moveStep = _scrollSpeed * Time.deltaTime;

        _totalOffsetZ += moveStep;
        _movedDistance += moveStep;

        foreach (var row in _rowLines)
        {
            if (row != null)
            {
                row.transform.position += Vector3.back * moveStep;
            }
        }

        if (_movedDistance >= _cellSizeZ)
        {
            _movedDistance -= _cellSizeZ;
            ScrollProcess();
        }
    }

    private void ScrollProcess()
    {
        RemoveBottomRow();
        SpawnTopRow();
    }

    private void RemoveBottomRow()
    {
        if (_rowLines.Count == 0) return;

        var rowObject = _rowLines.Dequeue();

        for (int x = _presenter.GetMinColumn(); x <= _presenter.GetMaxColumn(); x++)
        {
            var node = new Vector2Int(x, _bottomRowIndex);
            _presenter.RemoveGameObject(node);
            _presenter.RemoveGridCell(node);
        }

        Destroy(rowObject);
        _bottomRowIndex++;
    }

    private void SpawnTopRow()
    {
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
