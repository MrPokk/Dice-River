using TMPro;
using UINotDependence.Core;
using UnityEngine;

public class UIDistanceTraveledElement : UIPopup
{
    public TMP_Text text;

    public override void Open()
    {
        text.text = $"{Mathf.FloorToInt(0)} m";
    }

    public void UpdateDistance(float distance)
    {
        text.text = $"{Mathf.FloorToInt(distance)} m";
    }

}
