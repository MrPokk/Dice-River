using System.Linq;
using BitterECS.Integration;
using UnityEngine;

public class GenerationWorld : MonoBehaviour
{
    private MonoGridPresenter _monoGridPresenter;
    private Transform _worldParent;

    [SerializeField] private int _minRiverWidth = 12;
    [SerializeField] private float _noiseScale = 0.12f;
    [SerializeField] private float _meanderAmount = 4f;

    [SerializeField] private float _seed;
    [SerializeField] private int _minX;
    [SerializeField] private int _maxX;

    public void Config(MonoGridPresenter gridRaft)
    {
        _monoGridPresenter = gridRaft;
        _worldParent = new GameObject("GridWorldParent").transform;

        var nodes = gridRaft.GetGridNodes();
        if (nodes != null && nodes.Count > 0)
        {
            _minX = nodes.Keys.Min(k => k.x);
            _maxX = nodes.Keys.Max(k => k.x);
        }
        else
        {
            _minX = -10;
            _maxX = 10;
        }

        _seed = Random.value * 10000f;

        for (int i = -10; i < 20; i++)
        {
            GenerateRiverRow(i);
        }
    }

    private void GenerateRiverRow(int yIndex)
    {
        float noise = Mathf.PerlinNoise(_seed, yIndex * _noiseScale) * 2 - 1;
        int centerOffset = Mathf.RoundToInt(noise * _meanderAmount);

        int halfWidth = _minRiverWidth / 2;
        int leftWaterBorder = centerOffset - halfWidth;
        int rightWaterBorder = centerOffset + halfWidth;

        for (int x = _minX; x <= _maxX; x++)
        {
            if (x <= leftWaterBorder || x >= rightWaterBorder)
            {
                var shorePrefab = new Loader<ProviderEcs>(DicesPaths.GENERAL_DICE).GetPrefab();

                _monoGridPresenter.InitializeGameObject(new Vector2Int(x, yIndex), shorePrefab, out _, _worldParent);
            }
        }
    }

}
