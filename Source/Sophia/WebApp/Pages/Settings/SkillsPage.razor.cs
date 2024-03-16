namespace Sophia.WebApp.Pages.Settings;

public partial class SkillsPage {
    private IReadOnlyList<SkillData> _skills = [];
    private SkillData? _selectedSkill;
    private bool _showSkillModal;
    private bool _showDeleteConfirmation;


    [Inject]
    public required ISkillsService SkillsService { get; set; }

    protected override async Task OnInitializedAsync()
        => _skills = await SkillsService.GetList();

    private void OpenSkillModal(SkillData? skill = null) {
        _selectedSkill = skill ?? new();
        _showSkillModal = true;
    }

    private void CloseSkillDialog() {
        _selectedSkill = null;
        _showSkillModal = false;
    }

    private async Task SaveSkill() {
        if (_selectedSkill!.Id == 0) await SkillsService.Add(_selectedSkill);
        else await SkillsService.Update(_selectedSkill);
        _skills = await SkillsService.GetList();
        CloseSkillDialog();
    }

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
