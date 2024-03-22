namespace Sophia.WebClient.Components.Dialogs;

public partial class ChatSetupDialog {
    [Inject] public required IPersonasRemoteService PersonasService { get; set; }
    [Inject] public required IProvidersRemoteService ProvidersService { get; set; }

    [Parameter] public EventCallback OnStart { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public ChatData Chat { get; set; } = new();

    private IReadOnlyList<PersonaData> _personas = [];
    private IDictionary<string, string> _models = new Dictionary<string, string>();

    protected override async Task OnInitializedAsync() {
        _personas = await PersonasService.GetList();
        Chat.Agent.Persona = _personas.FirstOrDefault() ?? new();

        var providers = await ProvidersService.GetList();
        _models = providers.SelectMany(p => p.Models.Select(m => new {
            m.Key,
            Value = $"{m.Name} ({p.Name})",
        })).ToDictionary(k => k.Key, v => v.Value);
        Chat.Agent.Model = _models.Keys.FirstOrDefault() ?? string.Empty;
    }

    private void UpdateTemperature(ChangeEventArgs e)
        => Chat.Agent.Temperature = Convert.ToDouble(e.Value);

    private void Save() => OnStart.InvokeAsync();
    private void Cancel() => OnCancel.InvokeAsync();
}
