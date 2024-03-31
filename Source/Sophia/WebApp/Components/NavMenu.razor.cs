namespace Sophia.WebApp.Components;

public partial class NavMenu : IDisposable {
    private string? _currentUrl;
    private bool _collapseNavMenu;

    private string? CollapseCssClass => _collapseNavMenu ? "collapsed" : null;
    //private string? NavMenuHeaderCssClass => _collapseNavMenu ? "collapse-header" : "collapse-header show";
    //private string? NavMenuItemsCssClass => _collapseNavMenu ? "collapse" : "collapse show";
    //private string? NavMenuItemCssClass => _collapseNavMenu ? "collapse-item" : "collapse-item show";

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override void OnInitialized() {
        _currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void ToggleNavMenu() => _collapseNavMenu = !_collapseNavMenu;

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e) {
        _currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    public void Dispose() {
        NavigationManager.LocationChanged -= OnLocationChanged;
        GC.SuppressFinalize(this);
    }
}
