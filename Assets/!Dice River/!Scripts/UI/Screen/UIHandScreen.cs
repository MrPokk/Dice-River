using UINotDependence.Core;
using UnityEngine;

public class UIHandScreen : UIScreen
{
    public Transform handContainer;
    public void Bind(HandControllerDice handController)
    {
        handController.SetContainer(handContainer);
    }
}
