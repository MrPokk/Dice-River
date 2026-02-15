using System;
using UINotDependence.Core;
using UnityEngine;
using UnityEngine.UI;

public class UIRestartButtonComponent : MonoBehaviour, IUIProvider
{
    public GameObject UIObject => gameObject;
    private Button _button;

    private void Awake()
    {
        _button = gameObject.GetComponent<Button>();
    }

    public void AddListener(Action actionClick)
    {
        _button.onClick.AddListener(actionClick.Invoke);
    }

    public void RemoveListener(Action actionClick)
    {
        _button.onClick.RemoveListener(actionClick.Invoke);
    }

    public void Refresh()
    { }

    public void SetSelectNeighbors(GameObject upNeighbour, GameObject downNeighbour, GameObject leftNeighbour, GameObject rightNeighbour)
    { }
}
