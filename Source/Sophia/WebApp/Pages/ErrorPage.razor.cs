namespace Sophia.WebApp.Pages;

public partial class ErrorPage {
    [Inject]
    public required IWebHostEnvironment Environment { get; set; }

    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    private string? RequestId { get; set; }
    private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    protected override void OnInitialized() => RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
}
