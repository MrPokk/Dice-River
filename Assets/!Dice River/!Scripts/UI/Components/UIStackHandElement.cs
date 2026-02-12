using UINotDependence.Core;
using UnityEngine;

public class UIStackHandElement : UIPopup
{
    public Transform handStackContainer;
    public void Bind(HandStackControllerDice handStackController)
    {
        handStackController.SetContainer(handStackContainer);
    }
}
