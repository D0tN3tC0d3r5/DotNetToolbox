namespace AI.Sample.UserProfile.Commands;

public class UserProfileViewCommand(IHasChildren parent, IUserProfileHandler handler)
    : Command<UserProfileViewCommand>(parent, "Info", n => {
        n.Aliases = ["i"];
        n.Description = "Display the User Profile.";
    }) {
    protected override Result Execute() => this.HandleCommand(() => {
        Logger.LogInformation("Executing UserProfile->View command...");
        var user = handler.CurrentUser;
        if (user is null) {
            Logger.LogInformation("No user selected.");
            Output.WriteLine();

            return Result.Success();
        }

        Output.WriteLine("[yellow]User Information:[/]");
        Output.WriteLine($"[blue]Name:[/] {user.Name}");
        Output.WriteLine($"[blue]Preferred Language:[/] {user.Language}");
        Output.WriteLine();

        return Result.Success();
    }, "Error displaying the user profile.");

}
