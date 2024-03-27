namespace Sophia.WebClient.Components.Dialogs;

public partial class ChatSetupDialog {
    [Inject] public required IProvidersRemoteService ProvidersService { get; set; }
    [Inject] public required IPersonasRemoteService PersonasService { get; set; }

    [Parameter] public EventCallback OnStart { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public ChatData Chat { get; set; } = new();

    private IReadOnlyList<PersonaData> _personas = [];
    private IDictionary<string, string> _models = new Dictionary<string, string>();

    protected override async Task OnInitializedAsync() {
        _personas = await PersonasService.GetList();
        var providers = await ProvidersService.GetList();
        _models = providers.SelectMany(p => p.Models.Select(m => new {
            m.Key,
            Value = $"{m.Name} ({p.Name})",
        })).ToDictionary(k => k.Key, v => v.Value);
        var agent = new ChatAgentData {
            Number = Chat.Agents.Count,
            Persona = _personas[0],
            Provider = providers[0],
            Options = new() {
                Model = providers[0].Models[0].Key,
                ChatEndpoint = providers[0].Api.ChatEndpoint,
            },
        };
        Chat.Agents.Add(agent);
    }

    private void UpdateTemperature(ChangeEventArgs e)
        => Chat.Agents[0].Options.Temperature = Convert.ToDecimal(e.Value);

    private void Save() => OnStart.InvokeAsync();
    private void Cancel() => OnCancel.InvokeAsync();
}
