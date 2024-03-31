namespace Sophia.WebClient.Pages.Settings.Tools;

public partial class ToolPage {

    private string _argumentAction = PageAction.View;
    private IReadOnlyList<ToolData> _existingTools = [];
    private ToolData _tool = new();
    private EditContext _editContext;
    private ValidationMessageStore _validationMessageStore;
    private ArgumentData? _selectedArgument;
    private bool _showArgumentDialog;
    private bool _showDeleteConfirmationDialog;

    [Inject] public required IToolsRemoteService ToolsService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }

    [Parameter] public string Action { get; set; } = PageAction.View;
    public bool IsReadOnly => Action == PageAction.View;
    [Parameter] public int? Id { get; set; }

    public ToolPage() {
        _editContext = new(_tool);
        _validationMessageStore = new(_editContext);
    }

    protected override async Task OnInitializedAsync() {
        _existingTools = await ToolsService.GetList();
        _tool = await GetToolById(Id);
    }

    private async Task<ToolData> GetToolById(int? toolId)
        => toolId is null
               ? new()
               : await ToolsService.GetById(toolId.Value)
              ?? throw new InvalidOperationException();

    protected override void OnParametersSet() {
        _editContext = new(_tool);
        _validationMessageStore = new(_editContext);
        _editContext.OnValidationRequested += OnValidationRequested;
        base.OnParametersSet();
    }

    private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e) {
        _validationMessageStore.Clear(() => _tool);
        var result = _tool.ValidateSignature(_existingTools);
        if (result != null)
            _validationMessageStore.Add(() => _tool, result);
    }

    private void EnableEdit() => Action = PageAction.Edit;

    private async Task Save(EditContext editContext) {
        if (_tool.Id == 0)
            await ToolsService.Add(_tool);
        else
            await ToolsService.Update(_tool);
        Action = PageAction.View;
    }

    private async Task Cancel() {
        if (Action == PageAction.Edit)
            await CancelEdit();
        else
            GoBack();
        Action = PageAction.View;
    }

    private async Task CancelEdit() {
        Action = PageAction.View;
        _tool = await GetToolById(Id);
    }

    private void GoBack() => NavigationManager.NavigateTo("/Settings/Tools");

    private void Add() {
        _selectedArgument = new();
        _argumentAction = PageAction.Add;
        _showArgumentDialog = true;
    }

    private void View(ArgumentData argument) {
        _selectedArgument = argument;
        _argumentAction = PageAction.View;
        _showArgumentDialog = true;
    }

    private void Edit(ArgumentData argument) {
        _selectedArgument = argument;
        _argumentAction = PageAction.Edit;
        _showArgumentDialog = true;
    }

    private void CloseArgumentModal() {
        _showArgumentDialog = false;
        _selectedArgument = null;
    }

    private void SaveArgument() {
        _showArgumentDialog = false;
        if (_argumentAction == PageAction.Add)
            _tool.Arguments.Add(_selectedArgument!);
    }

    private void Delete(ArgumentData argument) {
        _selectedArgument = argument;
        _showDeleteConfirmationDialog = true;
    }

    private void CancelDelete() {
        _showDeleteConfirmationDialog = false;
        _selectedArgument = null;
    }

    private void ExecuteDelete() {
        _showDeleteConfirmationDialog = false;
        _tool.Arguments.Remove(_selectedArgument!);
    }

    private void MoveUp(ArgumentData argument) {
        var minimum = argument.IsRequired
                          ? 0
                          : _tool.Arguments.Count(a => a.IsRequired);
        var index = _tool.Arguments.IndexOf(argument);
        if (index <= minimum)
            return;
        _tool.Arguments.Remove(argument);
        _tool.Arguments.Insert(index - 1, argument);
    }

    private void MoveDown(ArgumentData argument) {
        var maximum = argument.IsRequired
                          ? _tool.Arguments.Count(a => a.IsRequired)
                          : _tool.Arguments.Count;
        var index = _tool.Arguments.IndexOf(argument);
        if (index >= maximum - 1)
            return;
        _tool.Arguments.Remove(argument);
        _tool.Arguments.Insert(index + 1, argument);
    }
}
