namespace Sophia.WebClient.Components.Dialogs;

public partial class FactDialog {
    [Parameter]
    public FactData Fact { get; set; } = default!;

    [Parameter]
    public EventCallback OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    [Parameter]
    public string Action { get; set; } = default!;

    private void Save() {
        if (Fact.Validate() is null) {
            OnSave.InvokeAsync();
        }
    }

    private void Cancel() => OnCancel.InvokeAsync();
}
