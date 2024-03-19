namespace Sophia.WebClient.Components.Dialogs;

public partial class ToolSelectionDialog {
    [Parameter]
    public IReadOnlyCollection<ToolData> AvailableTools { get; set; } = [];

    [Parameter]
    public List<ToolData> SelectedTools { get; set; } = [];

    [Parameter]
    public EventCallback<List<ToolData>> OnToolsSelected { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    public bool HasTools => AvailableTools.Count != 0;
    private string _searchText = string.Empty;

    private List<ToolData> FilteredTools
        => AvailableTools
          .Where(s => s.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                   || (s.Description?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ?? false))
          .ToList();

    private bool IsSelected(ToolData tool)
        => SelectedTools.Contains(tool);

    private void ToggleSelection(ToolData tool) {
        if (IsSelected(tool)) SelectedTools.Remove(tool);
        else SelectedTools.Add(tool);
    }

    private void Confirm()
        => OnToolsSelected.InvokeAsync(SelectedTools);

    private void Cancel()
        => OnCancel.InvokeAsync();
}
