using UnityEngine;

public class UISelectorElement : MonoBehaviour
{
    [SerializeField] private GameObject mouseSet;
    [SerializeField] private GameObject grabbingSet;

    public void SetVisualMode(bool canInteract)
    {
        if (grabbingSet.activeSelf != canInteract)
            grabbingSet.SetActive(canInteract);

        if (mouseSet.activeSelf == canInteract)
            mouseSet.SetActive(!canInteract);
    }

    public void SetVisualAllIcon(bool isVisible)
    {
        grabbingSet.SetActive(isVisible);
        mouseSet.SetActive(isVisible);
    }

    public void SetVisible(bool isVisible)
    {
        if (gameObject.activeSelf != isVisible)
            gameObject.SetActive(isVisible);
    }
}
