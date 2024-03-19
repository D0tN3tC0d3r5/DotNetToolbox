namespace Sophia.WebClient.Components.Dialogs;

public partial class ArgumentDialog {
    [Parameter]
    public PageAction Action { get; set; }

    [Parameter]
    public ArgumentData Argument { get; set; } = new();

    [Parameter]
    public EventCallback OnSave { get; set; }

    [Parameter]
    public EventCallback OnCancel { get; set; }

    private ArgumentData _argument = new();
    private EditContext _editContext;
    private ValidationMessageStore _validationMessageStore;

    public ArgumentDialog() {
        _editContext = new(_argument);
        _validationMessageStore = new(_editContext);
    }

    protected override void OnInitialized()
        => _argument = Argument;

    protected override void OnParametersSet() {
        _editContext = new(_argument);
        _validationMessageStore = new(_editContext);
        _editContext.OnValidationRequested += OnValidationRequested;
        base.OnParametersSet();
    }

    private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e) {
        _validationMessageStore.Clear(() => _argument.Choices);
        var result = _argument.ValidateChoices();
        if (result != null) _validationMessageStore!.Add(() => _argument.Choices, result);
    }

    private void Save() => OnSave.InvokeAsync();

    private void Cancel()
        => OnCancel.InvokeAsync();

    private void InsertOption(int index = -1) {
        if (index == _argument.Choices.Count - 1) _argument.Choices.Add(string.Empty);
        else _argument.Choices.Insert(index + 1, string.Empty);
    }

    private void RemoveOption(int index)
        => _argument.Choices.RemoveAt(index);
}
