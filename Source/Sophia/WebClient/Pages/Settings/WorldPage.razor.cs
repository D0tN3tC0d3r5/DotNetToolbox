using Timer = System.Timers.Timer;

namespace Sophia.WebClient.Pages.Settings;

public partial class WorldPage
    : IDisposable {
    private WorldData _world = new();
    private bool _isReadOnly = true;
    private string _dateTime = string.Empty;
    private Timer? _timer;

    private IReadOnlyCollection<ToolData> _availableTools = [];
    private List<ToolData> _toolSelectionBuffer = [];
    private bool _showToolSelectionDialog;

    private FactData? _selectedFact;
    private bool _showFactDialog;

    [Inject]
    public required IWorldRemoteService WorldService { get; set; }

    [Inject]
    public required IToolsRemoteService ToolsService { get; set; }

    [Inject]
    public required ILogger<WorldPage> Logger { get; set; }

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
        _world = await WorldService.GetWorld();
        _dateTime = _world.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
        _timer = new(1000) { AutoReset = true, Enabled = true };
        _timer.Elapsed += (_, _) => UpdateDateTime();
        _timer.Start();
    }

    public void Dispose() {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
        GC.SuppressFinalize(this);
    }

    private void UpdateDateTime() {
        _dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Logger.LogInformation("{DateTime}", _dateTime);
        InvokeAsync(StateHasChanged);
    }

    private void EnableEdit()
        => _isReadOnly = false;

    private async Task Cancel() {
        _world = await WorldService.GetWorld();
        _isReadOnly = true;
    }

    private async Task Save() {
        await WorldService.UpdateWorld(_world);
        _isReadOnly = true;
    }

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
            _world.Facts.Add(_selectedFact);
        }
        CloseFactDialog();
    }
    private void CloseFactDialog() {
        _showFactDialog = false;
        _selectedFact = null;
    }

    private void RemoveFact(FactData fact) {
        _world.Facts.Remove(fact);
        _selectedFact = null;
    }

    private async Task OpenToolSelectionDialog() {
        _availableTools = await ToolsService.GetList();
        _toolSelectionBuffer = [.. _world.Tools];
        _showToolSelectionDialog = true;
    }

    private void CloseToolSelectionDialog() {
        _toolSelectionBuffer = [];
        _showToolSelectionDialog = false;
    }

    private void FinishToolSelection(List<ToolData> tools) {
        _world.Tools = tools;
        CloseToolSelectionDialog();
    }

    private void RemoveTool(ToolData tool)
        => _world.Tools.Remove(tool);
}
