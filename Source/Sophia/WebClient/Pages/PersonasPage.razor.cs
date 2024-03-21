namespace Sophia.WebClient.Pages;

public partial class PersonasPage {
    [Inject]
    public required IPersonasService PersonasService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private IReadOnlyList<PersonaData> _personas = [];

    protected override async Task OnInitializedAsync()
        => _personas = await PersonasService.GetList();

    private void Add()
        => NavigationManager.NavigateTo("/Personas/Add");

    private void View(PersonaData persona)
        => NavigationManager.NavigateTo($"/Personas/View/{persona.Id}");
}
