using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine.EventSystems;

public class UIProvider : ProviderEcs<UIPresenter>, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Entity.AddFrameToEvent<IsPointerEnter>(new(eventData));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Entity.AddFrameToEvent<IsPointerExit>(new(eventData));

    }
}

public class UIPresenter : EcsPresenter
{ }
