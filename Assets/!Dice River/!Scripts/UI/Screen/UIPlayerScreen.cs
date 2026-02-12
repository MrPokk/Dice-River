using UINotDependence.Core;

public class UIPlayerScreen : UIScreen
{
    public UIHealthElement healthElement;
    public UIHandElement handElement;
    public UIStackHandElement stackHandElement;

    public void Bind(HandControllerDice handController, HandStackControllerDice stackController)
    {
        handElement.Bind(handController);
        stackHandElement.Bind(stackController);
    }

    public override void Open()
    {
        healthElement.Open();
        handElement.Open();
        stackHandElement.Open();
        base.Open();
    }

    public override void Close()
    {
        healthElement.Close();
        handElement.Close();
        stackHandElement.Close();
        base.Close();
    }
}
