using UINotDependence.Core;
using UnityEngine;

public class UIHandScreen : UIScreen
{
    public Transform handPoint;
    public void Bind(HandControllerDice handController)
    {
        handController.transform.position = handPoint.position;
        handController.transform.SetParent(transform);
    }
}
