namespace Sophia.WebClient.Pages.Settings.Providers;
public partial class ProvidersPage {
    [Inject] public IProvidersService ProvidersService { get; set; } = default!;
    [Inject] public required NavigationManager NavigationManager { get; set; }

    private IReadOnlyList<ProviderData> _providers = [];

    protected override async Task OnInitializedAsync()
        => _providers = await ProvidersService.GetList();

    private void GoBack() => NavigationManager.NavigateTo("/Settings");

    private void Add()
        => NavigationManager.NavigateTo("Settings/Provider/Add");

    private void View(ProviderData provider)
        => NavigationManager.NavigateTo($"Settings/Provider/View/{provider.Id}");
}
