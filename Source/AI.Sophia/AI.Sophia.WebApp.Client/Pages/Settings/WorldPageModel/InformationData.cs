namespace Sophia.WebApp.Client.Pages.Settings.WorldPageModel;

public class InformationData(Information information) {
    public string? Value { get; set; } = information.Value;
    public string ValueTemplate { get; set; } = information.ValueTemplate;
    public string NullText { get; set; } = information.DefaultText;

    public Information ToWorld()
        => new() {
            Value = Value,
            ValueTemplate = ValueTemplate,
            DefaultText = NullText,
        };
}
