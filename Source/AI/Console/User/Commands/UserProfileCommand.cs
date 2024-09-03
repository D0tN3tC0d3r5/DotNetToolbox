namespace AI.Sample.Users.Commands;

public class UserProfileCommand
    : Command<UserProfileCommand> {
    public UserProfileCommand(IHasChildren parent)
        : base(parent, "User", []) {
        Description = "Manage user profile.";

        AddCommand<UserSetCommand>();
        AddCommand<HelpCommand>();
    }

    protected override async Task<Result> Execute(CancellationToken ct = default) {
        var choice = Input.SelectionPrompt("What would you like to do?")
                          .ConvertWith(MapTo)
                          .AddChoices("Set",
                                      "Info",
                                      "Help",
                                      "Back",
                                      "Exit").Show();

        var userHandler = Application.Services.GetRequiredService<IUserHandler>();
        var command = choice switch {
            "Set" => new UserSetCommand(this, userHandler),
            "Info" => new UserInfoCommand(this, userHandler),
            "Help" => new HelpCommand(this),
            "Exit" => new ExitCommand(this),
            _ => (ICommand?)null,
        };
        return await (command?.Execute([], ct) ?? Result.SuccessTask());

        static string MapTo(string choice) => choice switch {
            "Set" => "Set User Profile",
            "Info" => "View User Profile",
            "Help" => "Help",
            "Back" => "Back",
            "Exit" => "Exit",
            _ => string.Empty,
        };
    }
}
