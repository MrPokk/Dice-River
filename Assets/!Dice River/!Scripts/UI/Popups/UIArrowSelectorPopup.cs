using UINotDependence.Core;

public class UIArrowSelectorPopup : UIPopup
{
    public override void Open()
    {
        UIAnimationComponent
        .Using(gameObject)
        .SetPresets(UIAnimationPresets.PopupOpen,
                    UIAnimationPresets.PopupClose)
        .PlayOpen();

        base.Open();
    }
}
