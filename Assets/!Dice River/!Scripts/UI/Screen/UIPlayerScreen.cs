using UINotDependence.Core;

public class UIPlayerScreen : UIScreen
{
    public UIHealthElement healthElement;
    public UIHandElement handElement;
    public UIStackHandElement stackHandElement;
    public UIDistanceTraveledElement distanceTraveledElement;

    public void Bind(HandControllerDice handController, HandStackControllerDice stackController, RiverScrolling scrolling)
    {
        handElement.Bind(handController);
        stackHandElement.Bind(stackController);
        scrolling.OnDistanceChanged += SetDistance;
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
        base.Close();
    }
}
