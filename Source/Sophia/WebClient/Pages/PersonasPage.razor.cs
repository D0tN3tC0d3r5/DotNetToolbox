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

    private void Edit(PersonaData persona)
        => NavigationManager.NavigateTo($"/Personas/Edit/{persona.Id}");

    private async Task Delete(PersonaData persona) {
        // TODO: Implement delete confirmation dialog
        await PersonasService.Delete(persona.Id);
        _personas = await PersonasService.GetList();
    }
}
