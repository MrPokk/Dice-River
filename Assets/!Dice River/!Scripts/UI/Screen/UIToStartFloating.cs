using UINotDependence.Core;

public class UIToStartFloating : UIScreen
{
    public override void Close()
    {
        UIAnimationComponent
        .Using(gameObject)
        .SetPresets(UIAnimationPresets.FadeIn, UIAnimationPresets.PopupClose)
        .PlayClose();

        base.Close();
    }
}
