namespace Sophia.WebClient.Pages.Personas;

public partial class PersonaPage {
    [Inject]
    public required IPersonasRemoteService PersonasService { get; set; }
    [Inject]
    public required IToolsRemoteService ToolsService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private PersonaData _persona = new();
    private PageAction _action;
    private IReadOnlyCollection<ToolData> _availableTools = [];
    private List<ToolData> _toolSelectionBuffer = [];
    private bool _showToolSelectionDialog;

    private FactData? _selectedFact;
    private bool _showFactDialog;

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

    private void GoBack() => NavigationManager.NavigateTo("/Personas");

    private async Task Save() {
        if (_persona.Id == 0) await PersonasService.Add(_persona);
        else await PersonasService.Update(_persona);
        _action = PageAction.View;
    }

    private Task Cancel() {
        if (_action == PageAction.Edit) return CancelEdit();
        GoBack();
        return Task.CompletedTask;
    }

    private async Task CancelEdit() {
        _action = PageAction.View;
        _persona = await GetPersonaById(PersonaId);
    }

    private void AddInstruction()
        => _persona.Instructions.Add(string.Empty);
    private void RemoveInstruction(int index)
        => _persona.Instructions.RemoveAt(index);

    private void AddFact() {
        _selectedFact = new();
        _showFactDialog = true;
    }

    private void EditFact(FactData fact) {
        _selectedFact = fact;
        _showFactDialog = true;
    }

    private void SaveFact() {
        if (_selectedFact!.Id == 0) {
            _persona.Facts.Add(_selectedFact);
        }
        CloseFactDialog();
    }
    private void CloseFactDialog() {
        _showFactDialog = false;
        _selectedFact = null;
    }

    private void RemoveFact(FactData fact) {
        _persona.Facts.Remove(fact);
        _selectedFact = null;
    }

    private async Task OpenToolSelectionDialog() {
        _availableTools = await ToolsService.GetList();
        _toolSelectionBuffer = [.. _persona.KnownTools];
        _showToolSelectionDialog = true;
    }

    private void CloseToolSelectionDialog() {
        _toolSelectionBuffer = [];
        _showToolSelectionDialog = false;
    }

    private void FinishToolSelection(List<ToolData> tools) {
        _persona.KnownTools = tools;
        CloseToolSelectionDialog();
    }

    private void RemoveTool(ToolData tool)
        => _persona.KnownTools.Remove(tool);
}
