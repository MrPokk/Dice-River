using TMPro;
using UINotDependence.Core;
using UnityEngine;

public class UIDistanceTraveledElement : UIPopup
{
    public TMP_Text text;

    public void UpdateDistance(float distance)
    {
        text.text = $"{Mathf.FloorToInt(distance)} m";
    }

}
