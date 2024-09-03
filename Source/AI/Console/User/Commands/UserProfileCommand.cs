namespace AI.Sample.Users.Commands;

public class UserProfileCommand
    : Command<UserProfileCommand> {
    public UserProfileCommand(IHasChildren parent)
        : base(parent, "User", []) {
        Description = "Manage user profile.";

        AddCommand<UserSetCommand>();
        AddCommand<HelpCommand>();
    }
}
