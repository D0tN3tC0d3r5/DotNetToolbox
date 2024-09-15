namespace AI.Sample.UserProfile.Commands;

public class UserProfileCommand(IHasChildren parent)
    : Command<UserProfileCommand>(parent, "UserProfile", n => {
        n.Description = "Manage User Profile.";
        n.AddCommand<UserProfileSetCommand>();
        n.AddCommand<HelpCommand>();
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        Logger.LogInformation("Executing UserProfile command...");
        var choice = await Input.BuildSelectionPrompt<string>("What would you like to do?")
                                .ConvertWith(MapTo)
                                .AddChoices(Commands.ToArray(c => c.Name))
                                .ShowAsync(lt);

        var command = Commands.FirstOrDefault(i => i.Name == choice);
        return command is null
            ? Result.Success()
            : await command.Execute([], lt);

        string MapTo(string choice) => Commands.FirstOrDefault(i => i.Name == choice)?.Description ?? string.Empty;
    }, "Error displaying the user profile menu.", ct);
}
