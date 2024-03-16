using Sophia.Models.Skills;
using Sophia.Models.Worlds;

using Timer = System.Timers.Timer;

namespace Sophia.WebClient.Pages.Settings;

public partial class WorldPage {
    private WorldData _world = new();
    private bool _isReadOnly = true;
    private string _dateTime = string.Empty;
    private Timer? _timer;

    private SkillData? _selectedSkill;
    private bool _showSkillModal;
    private bool _showDeleteConfirmation;

    [Inject]
    public required IWorldService WorldService { get; set; }

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

    private async Task CancelEdit() {
        _world = await WorldService.GetWorld();
        _isReadOnly = true;
    }

    private async Task SaveWorld() {
        await WorldService.UpdateWorld(_world);
        _isReadOnly = true;
    }

    private void AddInfo()
        => _world.AdditionalInformation.Add(new());

    private void DeleteInfo(InformationData info)
        => _world.AdditionalInformation.Remove(info);

    private void OpenSkillModal(SkillData? skill) {
        _selectedSkill = skill ?? new SkillData();
        _showSkillModal = true;
    }

    private void CloseSkillModal() {
        _selectedSkill = new();
        _showSkillModal = false;
    }

    private void SaveSkill() {
        var index = _world.Skills.FindIndex(s => s.Id == _selectedSkill!.Id);
        if (index != -1) _world.Skills[index] = _selectedSkill!;
        else _world.Skills.Add(_selectedSkill!);
        CloseSkillModal();
    }

    private void DeleteSkill(SkillData skill) {
        _selectedSkill = skill;
        _showDeleteConfirmation = true;
    }

    private void CancelDelete() {
        _selectedSkill = null;
        _showDeleteConfirmation = false;
    }

    private void ExecuteDelete() {
        _world.Skills.Remove(_selectedSkill!);
        _selectedSkill = null;
        _showDeleteConfirmation = false;
    }
}
