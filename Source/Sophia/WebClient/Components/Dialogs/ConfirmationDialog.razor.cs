namespace Sophia.WebClient.Components.Dialogs;

public partial class ConfirmationDialog {
    [Parameter]
    public string Message { get; set; } = "";

    [Parameter]
    public EventCallback OnConfirm { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    private void Confirm() => OnConfirm.InvokeAsync();

    private void Cancel() => OnCancel.InvokeAsync();
}
