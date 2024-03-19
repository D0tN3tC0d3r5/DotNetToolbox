﻿namespace Sophia.WebClient.Pages.Settings;

public partial class ToolsPage {
    private IReadOnlyList<ToolData> _tools = [];
    private ToolData? _selectedTool;
    private bool _showDeleteConfirmation;

    [Inject]
    public required IToolsService ToolsService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
        => _tools = await ToolsService.GetList();

    private void View(ToolData tool)
        => NavigationManager.NavigateTo($"/Settings/Tool/{PageAction.View}/{tool.Id}");

    private void Add()
        => NavigationManager.NavigateTo($"/Settings/Tool/{PageAction.Add}/0");

    private void Edit(ToolData tool)
        => NavigationManager.NavigateTo($"/Settings/Tool/{PageAction.Edit}/{tool.Id}");

    private void Delete(ToolData tool) {
        _selectedTool = tool;
        _showDeleteConfirmation = true;
    }

    private void CancelDelete() {
        _selectedTool = null;
        _showDeleteConfirmation = false;
    }

    private async Task ExecuteDelete() {
        await ToolsService.Delete(_selectedTool!.Id);
        _tools = await ToolsService.GetList();
        _selectedTool = null;
        _showDeleteConfirmation = false;
    }
}
