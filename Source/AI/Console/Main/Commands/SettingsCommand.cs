namespace AI.Sample.Main.Commands;

public class SettingsCommand(IHasChildren parent, IOptions<LolaSettings> settings)
    : Command<SettingsCommand>(parent, "Settings", n => {
        n.Aliases = ["set"];
        n.Description = "Display the current configuration of Lola.";
    }) {
    private readonly LolaSettings _settings = settings.Value;

    protected override Result Execute() => this.HandleCommand(() => {
        Logger.LogInformation("Executing Settings command...");
        DrawTable();
        return Result.Success();
    }, "Error displaying the settings.");

    private void DrawTable() {
        var table = new Table();
        table.AddColumn("Setting");
        table.AddColumn("Value");
        table.AddRow("Default AI Provider", _settings.DefaultAIProvider);
        table.AddRow("Available Models", string.Join(", ", _settings.AvailableModels));
        Output.Write(table);
        Output.WriteLine();
    }
}
