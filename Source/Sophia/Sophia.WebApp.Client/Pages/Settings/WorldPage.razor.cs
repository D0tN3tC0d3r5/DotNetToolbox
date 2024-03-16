using Timer = System.Timers.Timer;

namespace Sophia.WebClient.Pages.Settings;

public partial class WorldPage {
    private WorldData _world = default!;
    private bool _isReadOnly = true;
    private string _dateTime = string.Empty;
    private Timer? _timer;

    [Inject]
    public required IWorldService WorldService { get; set; }

    [Inject]
    public required ILogger<WorldPage> Logger { get; set; }

    protected override async Task OnInitializedAsync() {
        await base.OnInitializedAsync();
        _world = new WorldData();
        _dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var world = await WorldService.GetWorld();
        _world = new WorldData();
        _timer = new(1000) { AutoReset = true, Enabled = true };
        _timer.Elapsed += (_, _) => UpdateDateTime();
        _timer.Start();
    }

    public ValueTask DisposeAsync() {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
        return ValueTask.CompletedTask;
    }

    private void UpdateDateTime() {
        _dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Logger.LogInformation("{DateTime}", _dateTime);
        InvokeAsync(StateHasChanged);
    }

    private void EnableEdit()
        => _isReadOnly = false;

    private async Task CancelEdit() {
        _world = await WorldService.GetWorld();
        _isReadOnly = true;
    }

    private async Task SaveWorld() {
        await WorldService.UpdateWorld(_world);
        _isReadOnly = true;
    }
}
