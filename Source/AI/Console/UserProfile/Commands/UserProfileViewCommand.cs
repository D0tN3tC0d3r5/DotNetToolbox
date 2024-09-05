namespace AI.Sample.UserProfile.Commands;

public class UserProfileViewCommand : Command<UserProfileViewCommand> {
    private readonly IUserProfileHandler _handler;

    public UserProfileViewCommand(IHasChildren parent, IUserProfileHandler handler)
        : base(parent, "Info", ["i"]) {
        _handler = handler;
        Description = "Display the User Profile.";
    }

    protected override Result Execute() {
        var user = _handler.Get();
        if (user is null) {
            Logger.LogInformation("No user selected.");
            Output.WriteLine();

            return Result.Success();
        }

        Output.WriteLine("[yellow]User Information:[/]");
        Output.WriteLine($"[blue]Name:[/] {user.Name}");
        Output.WriteLine($"[blue]Preffered Language:[/] {user.Language}");
        Output.WriteLine();

        return Result.Success();
    }
}
