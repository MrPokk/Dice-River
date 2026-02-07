using UnityEngine;

public class SpriteSideComponent : MonoBehaviour
{
    public GameObject frontSidesObject;
    public GameObject leftSidesObject;
    public GameObject rightSidesObject;

    public void ToggleFront()
    {
        frontSidesObject.SetActive(!frontSidesObject.activeSelf);
    }

    public void ToggleLeft()
    {
        leftSidesObject.SetActive(!leftSidesObject.activeSelf);
    }

    public void ToggleRight()
    {
        rightSidesObject.SetActive(!rightSidesObject.activeSelf);
    }
}
