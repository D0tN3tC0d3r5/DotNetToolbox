namespace Sophia.WebApp.Client.Pages.Settings.WorldPageModel;

public class ArgumentData(Argument argument) {
    public string Name { get; set; } = argument.Name;
    public string Type { get; set; } = argument.Type;
    public string? Description { get; set; } = argument.Description;
    public string[]? Options { get; set; } = argument.Options;
    public bool IsRequired { get; set; } = argument.IsRequired;

    public Argument ToWorld()
        => new(Name, Type, Options, IsRequired, Description);
}
