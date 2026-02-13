using UnityEngine.EventSystems;

public struct IsPointerExit
{
    public PointerEventData eventData;

    public IsPointerExit(PointerEventData eventData)
    {
        this.eventData = eventData;
    }
}
