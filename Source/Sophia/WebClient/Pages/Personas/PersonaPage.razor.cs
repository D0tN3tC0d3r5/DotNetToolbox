namespace Sophia.WebClient.Pages.Personas;

public partial class PersonaPage {
    [Inject] public required IPersonasRemoteService PersonasService { get; set; }
    [Inject] public required IToolsRemoteService ToolsService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }

    private PersonaData _persona = new();
    private IReadOnlyCollection<ToolData> _availableTools = [];
    private List<ToolData> _toolSelectionBuffer = [];

    private bool _showToolSelectionDialog;
    private bool _showDeleteConfirmationDialog;

    [Parameter] public string Action { get; set; } = PageAction.View;
    [Parameter] public int? PersonaId { get; set; }
    private bool IsReadOnly => Action == PageAction.View;

    protected override async Task OnInitializedAsync()
        => _persona = await GetPersonaById(PersonaId);

    private async Task<PersonaData> GetPersonaById(int? personaId)
        => personaId is null
               ? new()
               : await PersonasService.GetById(personaId.Value)
              ?? throw new InvalidOperationException();

    private void EnableEdit() => Action = PageAction.Edit;

    private void GoBack() => NavigationManager.NavigateTo("/Personas");

    private async Task Save() {
        if (_persona.Id == 0) await PersonasService.Add(_persona);
        else await PersonasService.Update(_persona);
        Action = PageAction.View;
    }

    private Task Cancel() {
        if (Action == PageAction.Edit) return CancelEdit();
        GoBack();
        return Task.CompletedTask;
    }

    private async Task CancelEdit() {
        Action = PageAction.View;
        _persona = await GetPersonaById(PersonaId);
    }

    private void AddFact()
          => _persona.Facts.Add(string.Empty);
    private void UpdateFact(int index, object? newValue) {
        if (newValue is not string newText) return;
        if (string.IsNullOrWhiteSpace(newText)) return;
        if (_persona.Facts.Contains(newText)) return;
        _persona.Facts[index] = newText;
    }
    private void RemoveFact(int index)
        => _persona.Facts.RemoveAt(index);

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

    private void Delete()
        => _showDeleteConfirmationDialog = true;

    private void CancelDelete()
        => _showDeleteConfirmationDialog = false;

    private async Task ExecuteDelete() {
        _showDeleteConfirmationDialog = false;
        await PersonasService.Delete(_persona.Id);
        NavigationManager.NavigateTo("/Personas");
    }
}
