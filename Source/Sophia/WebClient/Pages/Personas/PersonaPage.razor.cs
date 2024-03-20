namespace Sophia.WebClient.Pages.Personas;

public partial class PersonaPage {
    [Inject]
    public required IPersonasRemoteService PersonasService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private PersonaData _persona = new();
    private bool _showToolSelectionDialog;
    private PageAction _action;

    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "<Pending>")]
    public string Action {
        get => _action.ToString();
        set => _action = Enum.Parse<PageAction>(value);
    }
    public bool IsReadOnly => _action == PageAction.View;

    [Parameter]
    public int? PersonaId { get; set; }

    protected override async Task OnInitializedAsync()
        => _persona = await GetPersonaById(PersonaId);

    private async Task<PersonaData> GetPersonaById(int? personaId)
        => personaId is null
               ? new()
               : await PersonasService.GetById(personaId.Value)
              ?? throw new InvalidOperationException();

    private void EnableEdit() => _action = PageAction.Edit;

    private async Task Save() {
        if (_persona.Id == 0)
            await PersonasService.Add(_persona);
        else
            await PersonasService.Update(_persona);

        _action = PageAction.View;
    }

    private void Cancel() => NavigationManager.NavigateTo("/Personas");
    private void AddInstruction() => _persona.Instructions.Add(string.Empty);
    private void RemoveInstruction(string instruction) => _persona.Instructions.Remove(instruction);
    private void AddFact() => _persona.Facts.Add(new FactData());
    private void RemoveFact(FactData fact) => _persona.Facts.Remove(fact);
    private void OpenToolSelectionDialog() => _showToolSelectionDialog = true;
    private void CloseToolSelectionDialog() => _showToolSelectionDialog = false;
    private void RemoveTool(ToolData tool) => _persona.KnownTools.Remove(tool);
}
