namespace Sophia.WebClient.Pages.Settings;

public partial class SkillPage {
    [Parameter]
    public int SkillId { get; set; }

    private PageAction _action;
    [Parameter]
    public string Action {
        get => _action.ToString();
        set => _action = Enum.Parse<PageAction>(value); }

    private PageAction _argumentAction = PageAction.View;

    private bool IsReadOnly => _action == PageAction.View;

    [Inject]
    public required ISkillsService SkillsService { get; set; }

    [Inject]
    public required NavigationManager NavigationManager { get; set; }

    private SkillData _skill = new();
    private ArgumentData? _selectedArgument;
    private bool _showArgumentModal;
    private bool _showDeleteConfirmation;

    protected override async Task OnInitializedAsync()
        => _skill = await SkillsService.GetById(SkillId) ?? new SkillData();

    private void EnableEdit() => _action = PageAction.Edit;

    private async Task Save() {
        if (_skill.Id == 0) await SkillsService.Add(_skill);
        else await SkillsService.Update(_skill);
        _action = PageAction.View;
    }

    private Task Cancel() {
        if (_action == PageAction.Edit) return CancelEdit();
        GoBack();
        return Task.CompletedTask;
    }

    private async Task CancelEdit() {
        _action = PageAction.View;
        _skill = await SkillsService.GetById(SkillId) ?? new SkillData();
    }

    private void GoBack() => NavigationManager.NavigateTo("/Settings/Skills");

    private void Add() {
        _selectedArgument = new();
        _argumentAction = PageAction.Add;
        _showArgumentModal = true;
    }

    private void View(ArgumentData argument) {
        _selectedArgument = argument;
        _argumentAction = PageAction.View;
        _showArgumentModal = true;
    }

    private void Edit(ArgumentData argument) {
        _selectedArgument = argument;
        _argumentAction = PageAction.Edit;
        _showArgumentModal = true;
    }

    private void CloseArgumentModal() {
        _showArgumentModal = false;
        _selectedArgument = null;
    }

    private void SaveArgument() {
        _showArgumentModal = false;
        if (_argumentAction == PageAction.Add)
            _skill.Arguments.Add(_selectedArgument!);
    }

    private void Delete(ArgumentData argument) {
        _selectedArgument = argument;
        _showDeleteConfirmation = true;
    }

    private void CancelDelete() {
        _showDeleteConfirmation = false;
        _selectedArgument = null;
    }

    private void ExecuteDelete() {
        _showDeleteConfirmation = false;
        _skill.Arguments.Remove(_selectedArgument!);
    }

    private void MoveUp(ArgumentData argument) {
        var minimum = argument.IsRequired
                          ? 0
                          : _skill.Arguments.Count(a => a.IsRequired);
        var index = _skill.Arguments.IndexOf(argument);
        if (index <= minimum) return;
        _skill.Arguments.Remove(argument);
        _skill.Arguments.Insert(index - 1, argument);
    }

    private void MoveDown(ArgumentData argument) {
        var maximum = argument.IsRequired
                          ? _skill.Arguments.Count(a => a.IsRequired)
                          : _skill.Arguments.Count;
        var index = _skill.Arguments.IndexOf(argument);
        if (index >= maximum - 1) return;
        _skill.Arguments.Remove(argument);
        _skill.Arguments.Insert(index + 1, argument);
    }
}
