namespace Sophia.WebClient.Components.Dialogs;

public partial class ChatSetupDialog {
    [Inject] public required IPersonasRemoteService PersonasService { get; set; }
    [Inject] public required IProvidersRemoteService ProvidersService { get; set; }

    [Parameter] public EventCallback OnStart { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public ChatData Chat { get; set; } = new();

    private IReadOnlyList<PersonaData> _personas = [];
    private IDictionary<string, string> _models = new Dictionary<string, string>();
    private string _selectedModel = string.Empty;

    protected override async Task OnInitializedAsync() {
        _personas = await PersonasService.GetList();
        Chat.Agent.Persona = _personas.Count == 0 ? new() : _personas[0];

        var providers = await ProvidersService.GetList();
        _models = providers.SelectMany(p => p.Models.Select(m => new {
            Key = m.Name,
            Value = $"{m.Name} ({p.Name})",
        })).ToDictionary(k => k.Key, v => v.Value);
        Chat.Agent.Options.Model = _models.Keys.First();
        Chat.Agent.Provider = providers.First(p => p.Models.Any(m => m.Name == Chat.Agent.Options.Model));
    }

    private void UpdateTemperature(ChangeEventArgs e)
        => Chat.Agent.Options.Temperature = Convert.ToDecimal(e.Value);

    private void Save() => OnStart.InvokeAsync();
    private void Cancel() => OnCancel.InvokeAsync();
}
