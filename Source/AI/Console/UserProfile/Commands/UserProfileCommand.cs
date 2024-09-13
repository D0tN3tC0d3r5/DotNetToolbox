namespace AI.Sample.UserProfile.Commands;

public class UserProfileCommand
    : Command<UserProfileCommand> {
    public UserProfileCommand(IHasChildren parent)
        : base(parent, "UserProfile", []) {
        Description = "Manage User Profile.";

        AddCommand<UserProfileSetCommand>();
        AddCommand<HelpCommand>();
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async (ct) => {
        var choice = await Input.BuildSelectionPrompt<string>("What would you like to do?")
                                .ConvertWith(MapTo)
                                .AddChoices("Info",
                                            "Set",
                                            "Help",
                                            "Back",
                                            "Exit")
                                .ShowAsync(ct);

        var userHandler = Application.Services.GetRequiredService<IUserProfileHandler>();
        var command = choice switch {
            "Info" => new UserProfileViewCommand(this, userHandler),
            "Set" => new UserProfileSetCommand(this, userHandler),
            "Help" => new HelpCommand(this),
            "Exit" => new ExitCommand(this),
            _ => (ICommand?)null,
        };
        return command is null
            ? Result.Success()
            : await command.Execute([], ct);

        static string MapTo(string choice) => choice switch {
            "Info" => "View User Profile",
            "Set" => "Set User Profile",
            "Help" => "Help",
            "Back" => "Back",
            "Exit" => "Exit",
            _ => string.Empty,
        };
    }, "Error displaying the user profile menu.", ct);
}
