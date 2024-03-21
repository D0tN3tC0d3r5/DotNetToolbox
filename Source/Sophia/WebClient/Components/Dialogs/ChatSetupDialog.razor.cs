namespace Sophia.WebClient.Components.Dialogs;

public partial class ChatSetupDialog {
    [Parameter]
    public ChatData Chat { get; set; } = default!;

    [Parameter]
    public EventCallback OnStart { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    private readonly IReadOnlyList<string> _personas = [];
    private readonly IReadOnlyList<string> _models = [];

    private void Save() {
        if (Chat.Validate() is null)
            OnStart.InvokeAsync();
    }

    private void Cancel() => OnCancel.InvokeAsync();
}
