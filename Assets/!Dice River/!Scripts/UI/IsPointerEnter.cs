using UnityEngine.EventSystems;

public struct IsPointerEnter
{
    public PointerEventData eventData;

    public IsPointerEnter(PointerEventData eventData)
    {
        this.eventData = eventData;
    }
}
