using TMPro;
using UINotDependence.Core;
using UnityEngine;

public class UIDistanceTraveledElement : UIPopup
{
    public TMP_Text text;

    public override void Open()
    {
        text.text = $"{Mathf.FloorToInt(0)} m";
        gameObject.SetActive(false);
    }

    public void UpdateDistance(float distance)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        text.text = $"{Mathf.FloorToInt(distance)} m";
    }

}
