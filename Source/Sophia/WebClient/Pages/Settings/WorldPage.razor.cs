using Timer = System.Timers.Timer;

namespace Sophia.WebClient.Pages.Settings;

public partial class WorldPage {
    private WorldData _world = new();
    private bool _isReadOnly = true;
    private string _dateTime = string.Empty;
    private Timer? _timer;

    private IReadOnlyCollection<SkillData> _availableSkills = [];
    private List<SkillData> _skillSelectionBuffer = [];
    private bool _showSkillSelectionDialog;

    [Inject]
    public required IWorldService WorldService { get; set; }

    [Inject]
    public required ISkillsService SkillsService { get; set; }

    [Inject]
    public required ILogger<WorldPage> Logger { get; set; }

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
    }

    private void UpdateDateTime() {
        _dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Logger.LogInformation("{DateTime}", _dateTime);
        InvokeAsync(StateHasChanged);
    }

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

    private void AddInfo()
        => _world.AdditionalInformation.Add(new());

    private void DeleteInfo(InformationData info)
        => _world.AdditionalInformation.Remove(info);

    private async Task OpenSkillSelectionDialog() {
        _availableSkills = await SkillsService.GetList();
        _skillSelectionBuffer = [.. _world.Skills];
        _showSkillSelectionDialog = true;
    }

    private void CloseSkillSelectionDialog() {
        _skillSelectionBuffer = [];
        _showSkillSelectionDialog = false;
    }

    private void FinishSkillSelection(List<SkillData> skills) {
        _world.Skills = skills;
        CloseSkillSelectionDialog();
    }

    private void RemoveSkill(SkillData skill)
        => _world.Skills.Remove(skill);
}
