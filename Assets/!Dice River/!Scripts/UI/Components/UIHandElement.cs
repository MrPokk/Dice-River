using UINotDependence.Core;
using UnityEngine;

public class UIHandElement : UIPopup
{
    public Transform handContainer;
    public void Bind(HandControllerDice handController)
    {
        handController.SetContainer(handContainer);
    }
}
