using UINotDependence.Core;

public class UIPlayerScreen : UIScreen
{
    public UIHealthElement healthElement;
    public UIHandElement handElement;

    public void Bind(HandControllerDice handController)
    {
        handElement.Bind(handController);
    }

    public override void Open()
    {
        healthElement.Open();
        handElement.Open();
        base.Open();
    }

    public override void Close()
    {
        healthElement.Close();
        handElement.Close();
        base.Close();
    }
}
