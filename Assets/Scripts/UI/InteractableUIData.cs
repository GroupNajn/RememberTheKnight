public class InteractableUIData
{
    private string infoText;
    public string InfoText
    {
        get => infoText;
        set => infoText = value;
    }

    private string errorText;
    public string ErrorText
    {
        get => errorText;
        set => errorText = value;
    }
    private bool canInteract;

    public bool CanInteract
    {
        get => canInteract;
        set => canInteract = value;
    }
}