namespace Lola.UserProfile.Commands;

public class UserProfileCommand(IHasChildren parent)
    : Command<UserProfileCommand>(parent, "UserProfile", n => {
        n.Description = "Manage Your Profile";
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

        string MapTo(string item) => Commands.FirstOrDefault(i => i.Name == item)?.Description ?? string.Empty;
    }, "Error displaying the user profile menu.", ct);
}
