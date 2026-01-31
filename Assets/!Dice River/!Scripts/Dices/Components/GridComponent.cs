using System.Numerics;
using UnityEngine;

struct GridComponent
{
    public MonoGridPresenter gridPresenter;
    public Vector2Int currentPosition;

    public GridComponent(Vector2Int currentPosition, MonoGridPresenter gridPresenter)
    {
        this.currentPosition = currentPosition;
        this.gridPresenter = gridPresenter;
    }
}
