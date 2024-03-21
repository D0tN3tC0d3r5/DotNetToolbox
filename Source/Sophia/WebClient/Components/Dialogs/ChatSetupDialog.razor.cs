namespace Sophia.WebClient.Components.Dialogs;

public partial class ChatSetupDialog {
    [Inject] public required IPersonasRemoteService PersonasService { get; set; }

    [Parameter]
    public EventCallback OnStart { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    private readonly ChatData _chat = new();
    private IReadOnlyList<PersonaData> _personas = [];
    private IReadOnlyList<string> _models = [];
    private EditContext _editContext;
    private ValidationMessageStore _validationMessageStore;

    public ChatSetupDialog() {
        _editContext = new(_chat);
        _validationMessageStore = new(_editContext);
    }

    protected override async Task OnInitializedAsync() {
        _personas = await PersonasService.GetList();
        _models = ["GPT 4 Turbo", "Claude 3 Opus"];
        _chat.Agent.Model = _models[0];
    }

    protected override void OnParametersSet() {
        _editContext = new(_chat);
        _validationMessageStore = new(_editContext);
        _editContext.OnValidationRequested += OnValidationRequested;
        base.OnParametersSet();
    }

    private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e) {
    }

    private void Save() => OnStart.InvokeAsync();
    private void Cancel() => OnCancel.InvokeAsync();
}
