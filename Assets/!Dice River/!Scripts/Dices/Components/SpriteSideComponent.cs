using UnityEngine;

public class SpriteSideComponent : MonoBehaviour
{
    public GameObject frontSidesObject;
    public GameObject leftSidesObject;
    public GameObject rightSidesObject;

    public void SetFrontActive(bool isActive)
    {
        if (frontSidesObject.activeSelf != isActive)
            frontSidesObject.SetActive(isActive);
    }

    public void SetLeftActive(bool isActive)
    {
        if (leftSidesObject.activeSelf != isActive)
            leftSidesObject.SetActive(isActive);
    }

    public void SetRightActive(bool isActive)
    {
        if (rightSidesObject.activeSelf != isActive)
            rightSidesObject.SetActive(isActive);
    }

    public void ResetSides()
    {
        SetFrontActive(true);
        SetLeftActive(true);
        SetRightActive(true);
    }

    public void ToggleFront() => SetFrontActive(!frontSidesObject.activeSelf);
    public void ToggleLeft() => SetLeftActive(!leftSidesObject.activeSelf);
    public void ToggleRight() => SetRightActive(!rightSidesObject.activeSelf);
}
