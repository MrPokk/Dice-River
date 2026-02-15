using BitterECS.Core;
using BitterECS.Integration;
using UnityEngine.EventSystems;

public class UIProvider : ProviderEcs<UIPresenter>, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Entity.Has<IsNotRaycast>()) return;

        Entity.AddFrameToEvent<IsPointerEnter>(new(eventData));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Entity.Has<IsNotRaycast>()) return;

        Entity.AddFrameToEvent<IsPointerExit>(new(eventData));
    }
}

public class UIPresenter : EcsPresenter
{ }
