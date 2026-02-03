using BitterECS.Integration;
using UnityEngine;

public class RiverGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int _spawnDepth = 40;
    [SerializeField] private int _width = 6;
    [SerializeField] private float _zOffset = 6f;
    [SerializeField] private float _pathNoise = 6f;
    [SerializeField] private float _pathZScale = 0.1f;
    [SerializeField] private float _widthNoise = 5f;
    [SerializeField] private float _widthZScale = 0.1f;

    public int SpawnDepth => _spawnDepth;

    [SerializeField] private ProviderEcs _shorePrefab;

    public GameObject GenerateRow(MonoGridPresenter presenter, int r, Transform parent)
    {
        var indexColumnMin = presenter.GetMinColumn();
        var indexColumnMax = presenter.GetMaxColumn();
        var gridCenter = (indexColumnMin + indexColumnMax) / 2f;

        var rowLine = new GameObject($"Line_Row_Y{r}");
        rowLine.transform.SetParent(parent);

        var num = r + _zOffset;
        var pathShift = Mathf.Floor((Mathf.PerlinNoise(_pathZScale * num, 0.2f) - 0.5f) * _pathNoise);
        var dynamicWidth = Mathf.Floor(_width + Mathf.PerlinNoise(0.245f, _widthZScale * num) * _widthNoise);

        var leftBound = gridCenter + pathShift - dynamicWidth;
        var rightBound = gridCenter + pathShift + dynamicWidth;

        for (int x = indexColumnMin; x <= indexColumnMax; x++)
        {
            var node = new Vector2Int(x, r);

            if (x <= leftBound || x >= rightBound)
            {
                presenter.AddGridCell(node);
                presenter.InitializeGameObject(node, _shorePrefab, out _, rowLine.transform);
            }
        }

        return rowLine;
    }
}
