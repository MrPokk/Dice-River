using UINotDependence.Core;

public class UIPlayerScreen : UIScreen
{
    public UIHealthElement healthElement;
    public UIHandElement handElement;
    public UIStackHandElement stackHandElement;
    public UIDistanceTraveledElement distanceTraveledElement;

    private RiverScrollingSystem _riverScrollingSystem;

    public void Bind(HandControllerDice handController, HandStackControllerDice stackController, RiverScrollingSystem scrolling)
    {
        handElement.Bind(handController);
        stackHandElement.Bind(stackController);
        _riverScrollingSystem = scrolling;
        _riverScrollingSystem.OnDistanceChanged += SetDistance;
    }

    public override void Open()
    {
        healthElement ??= GetComponentInChildren<UIHealthElement>();
        handElement ??= GetComponentInChildren<UIHandElement>();
        stackHandElement ??= GetComponentInChildren<UIStackHandElement>();
        distanceTraveledElement ??= GetComponentInChildren<UIDistanceTraveledElement>();

        healthElement.Open();
        handElement.Open();
        stackHandElement.Open();
        distanceTraveledElement.Open();
        base.Open();
    }

    public void SetDistance(float value)
    {
        distanceTraveledElement.UpdateDistance(value);
    }

    public override void Close()
    {
        healthElement.Close();
        handElement.Close();
        stackHandElement.Close();
        distanceTraveledElement.Close();
        if (_riverScrollingSystem != null)
            _riverScrollingSystem.OnDistanceChanged -= SetDistance;
        base.Close();
    }
}
