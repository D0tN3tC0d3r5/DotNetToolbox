using Timer = System.Timers.Timer;

namespace Sophia.WebClient.Pages.Settings.World;

public partial class WorldPage
    : IDisposable {
    private WorldData _world = new();
    private bool _isReadOnly = true;
    private string _dateTime = string.Empty;
    private Timer? _timer;

    [Inject] public required IWorldRemoteService WorldService { get; set; }
    [Inject] public required NavigationManager NavigationManager { get; set; }

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
        InvokeAsync(StateHasChanged);
    }

    private void GoBack() => NavigationManager.NavigateTo("/Settings");

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

    private void AddFact()
        => _world.Facts.Add(string.Empty);
    private void UpdateFact(string oldValue, object? newValue) {
        if (newValue is not string newText) return;
        if (string.IsNullOrWhiteSpace(newText)) return;
        if (newText == oldValue) return;
        _world.Facts.Remove(oldValue);
        _world.Facts.Add(newText);
    }
    private void RemoveFact(string fact)
        => _world.Facts.Remove(fact);
}
