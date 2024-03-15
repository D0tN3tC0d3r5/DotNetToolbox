using Sophia.WebApp.Client.Pages.Settings.WorldPageModel;

using Timer = System.Timers.Timer;

namespace Sophia.WebApp.Client.Pages.Settings;

public partial class WorldPage {
    private WorldData _world = default!;
    private bool _isReadOnly = true;
    private string _dateTime = string.Empty;
    private Timer? _timer;

    //[Inject]
    //public required WorldService WorldService { get; set; }

    [Inject]
    public required ILogger<WorldPage> Logger { get; set; }

    protected override void OnInitialized() {
        _dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        _world = new(new DotNetToolbox.AI.Chats.World()); //WorldService.GetWorld();
        _timer = new(1000) { AutoReset = true, Enabled = true, };
        _timer.Elapsed += (_, _) => UpdateDateTime();
        _timer.Start();
    }
    public void Dispose() {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
    }

    private void UpdateDateTime() {
        _dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Logger.LogInformation("{DateTime}", _dateTime);
        InvokeAsync(StateHasChanged);
    }

    private void EnableEdit()
        => _isReadOnly = false;

    private void CancelEdit() {
        //_world = WorldService.GetWorld();
        _isReadOnly = true;
    }

    private void SaveWorld() {
        //WorldService.UpdateWorld(_world);
        _isReadOnly = true;
    }
}
