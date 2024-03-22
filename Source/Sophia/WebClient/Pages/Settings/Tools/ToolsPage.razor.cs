namespace Sophia.WebClient.Pages.Settings.Tools;

public partial class ToolsPage {
    private IReadOnlyList<ToolData> _tools = [];
    private ToolData? _selectedTool;
    private bool _showDeleteConfirmationDialog;

    [Inject]
    public required IToolsService ToolsService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
        => _tools = await ToolsService.GetList();

    private void GoBack() => NavigationManager.NavigateTo("/Settings");

    private void View(ToolData tool)
        => NavigationManager.NavigateTo($"/Settings/Tool/{PageAction.View}/{tool.Id}");

    private void Add()
        => NavigationManager.NavigateTo($"/Settings/Tool/{PageAction.Add}/0");

    private void Edit(ToolData tool)
        => NavigationManager.NavigateTo($"/Settings/Tool/{PageAction.Edit}/{tool.Id}");

    private void Delete(ToolData tool) {
        _selectedTool = tool;
        _showDeleteConfirmationDialog = true;
    }

    private void CancelDelete() {
        _selectedTool = null;
        _showDeleteConfirmationDialog = false;
    }

    private async Task ExecuteDelete() {
        await ToolsService.Delete(_selectedTool!.Id);
        _tools = await ToolsService.GetList();
        _selectedTool = null;
        _showDeleteConfirmationDialog = false;
    }
}
