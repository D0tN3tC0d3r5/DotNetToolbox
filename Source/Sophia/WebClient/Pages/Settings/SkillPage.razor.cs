namespace Sophia.WebClient.Pages.Settings;

public partial class SkillPage {
    [Parameter]
    public int SkillId { get; set; }

    [Parameter]
    public string Action { get; set; } = "view";

    private bool IsReadOnly => Action == "view";

    [Inject]
    public required ISkillsService SkillsService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private SkillData _skill = new();

    protected override async Task OnInitializedAsync()
        => _skill = await SkillsService.GetById(SkillId) ?? new SkillData();

    private void EnableEdit() => Action = "edit";

    private async Task SaveSkill() {
        if (_skill.Id == 0) await SkillsService.Add(_skill);
        else await SkillsService.Update(_skill);
        Action = "view";
    }

    private async Task CancelEdit() {
        Action = "view";
        _skill = (await SkillsService.GetById(SkillId)) ?? new SkillData();
    }

    private void GoBack() => NavigationManager.NavigateTo("/Settings/Skills");

    private void MoveArgumentUp(ArgumentData argument) {
        // Implement the logic to move the argument up in the list
    }

    private void MoveArgumentDown(ArgumentData argument) {
        // Implement the logic to move the argument down in the list
    }

    private void ToggleArgumentRequired(ArgumentData argument) {
        // Implement the logic to toggle the IsRequired property of the argument
        // and move it to the appropriate list (required or optional)
    }
}
