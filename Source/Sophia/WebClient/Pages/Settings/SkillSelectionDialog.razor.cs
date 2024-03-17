namespace Sophia.WebClient.Pages.Settings;

public partial class SkillSelectionDialog {
    [Parameter]
    public IReadOnlyCollection<SkillData> AvailableSkills { get; set; } = [];

    [Parameter]
    public List<SkillData> SelectedSkills { get; set; } = [];

    [Parameter]
    public EventCallback<List<SkillData>> OnSkillsSelected { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    public bool ShowDialog => AvailableSkills.Count != 0;
    private string _searchText = string.Empty;

    private List<SkillData> FilteredSkills
        => AvailableSkills
          .Where(s => s.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                   || (s.Description?.Contains(_searchText, StringComparison.OrdinalIgnoreCase) ?? false))
          .ToList();

    private bool IsSelected(SkillData skill) {
        return SelectedSkills.Contains(skill);
    }

    private void ToggleSelection(SkillData skill) {
        if (IsSelected(skill)) {
            SelectedSkills.Remove(skill);
        }
        else {
            SelectedSkills.Add(skill);
        }
    }

    private void Confirm() {
        OnSkillsSelected.InvokeAsync(SelectedSkills);
    }

    private void Cancel() {
        OnCancel.InvokeAsync();
    }
}
