namespace Sophia.WebClient.Components.Dialogs;

public partial class ChatSetupDialog {
    [Inject] public required IPersonasRemoteService PersonasService { get; set; }
    [Inject] public required IProvidersRemoteService ProvidersService { get; set; }

    [Parameter] public EventCallback OnStart { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }

    private readonly ChatData _chat = new();
    private IReadOnlyList<PersonaData> _personas = [];
    private IDictionary<string, string> _models = new Dictionary<string, string>();
    private EditContext _editContext;
    private ValidationMessageStore _validationMessageStore;

    public ChatSetupDialog() {
        _editContext = new(_chat);
        _validationMessageStore = new(_editContext);
    }

    protected override async Task OnInitializedAsync() {
        _personas = await PersonasService.GetList();
        var providers = await ProvidersService.GetList();
        _models = providers.SelectMany(p => p.Models.Select(m => new {
            m.Key,
            Value = $"{m.Name} ({p.Name})",
        })).ToDictionary(k => k.Key, v => v.Value);
        _chat.Agent.Model = _models.Keys.FirstOrDefault() ?? string.Empty;
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
