namespace Sophia.WebClient.Pages.Settings;

public partial class SkillsPage {
    private IReadOnlyList<SkillData> _skills = [];
    private SkillData? _selectedSkill;
    private bool _showDeleteConfirmation;


    [Inject]
    public required ISkillsService SkillsService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
        => _skills = await SkillsService.GetList();

    private void AddSkill()
        => NavigationManager.NavigateTo($"/Settings/Skills/0?Action=add");

    private void ViewSkill(SkillData skill)
        => NavigationManager.NavigateTo($"/Settings/Skills/{skill.Id}?Action=view");

    private void EditSkill(SkillData skill)
        => NavigationManager.NavigateTo($"/Settings/Skills/{skill.Id}?Action=edit");

    private void DeleteSkill(SkillData skill) {
        _selectedSkill = skill;
        _showDeleteConfirmation = true;
    }

    private void CancelDelete() {
        _selectedSkill = null;
        _showDeleteConfirmation = false;
    }

    private async Task ExecuteDelete() {
        await SkillsService.Delete(_selectedSkill!.Id);
        _skills = await SkillsService.GetList();
        _selectedSkill = null;
        _showDeleteConfirmation = false;
    }
}
