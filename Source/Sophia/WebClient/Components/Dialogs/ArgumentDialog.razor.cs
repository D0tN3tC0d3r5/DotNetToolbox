using Sophia.WebClient.Pages;

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

    protected override void OnInitialized()
        => _argument = Argument;

    private void Save()
        => OnSave.InvokeAsync();

    private void Cancel()
        => OnCancel.InvokeAsync();

    private void InsertOption(int index = -1) {
        if (index == _argument.Options.Count - 1) _argument.Options.Add(string.Empty);
        else _argument.Options.Insert(index + 1, string.Empty);
    }

    private void RemoveOption(int index)
        => _argument.Options.RemoveAt(index);
}
