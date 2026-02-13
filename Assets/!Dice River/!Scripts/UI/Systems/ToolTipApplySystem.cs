using BitterECS.Core;
using UINotDependence.Core;

public class ToolTipApplySystem : IEcsAutoImplement
{
    public Priority Priority => Priority.Medium;

    private EcsEvent _ecsEventEnter = new EcsEvent<UIPresenter>()
    .Subscribe<IsPointerEnter>(OnPointerEnter);

    private EcsEvent _ecsEventExit = new EcsEvent<UIPresenter>()
    .Subscribe<IsPointerExit>(OnPointerExit);

    private static void OnPointerEnter(EcsEntity entity)
    {
        ref var name = ref entity.Get<NameComponent>();
        ref var descriptor = ref entity.Get<DescriptorComponent>();
        ref var abilityDescriptor = ref entity.Get<AbilityDescriptorComponent>();

        var popup = UIController.OpenPopup<UITooltipPopup>();
        popup.Bind(name, descriptor, abilityDescriptor);
    }

    private static void OnPointerExit(EcsEntity entity)
    {
        UIController.ClosePopup<UITooltipPopup>();
    }
}
