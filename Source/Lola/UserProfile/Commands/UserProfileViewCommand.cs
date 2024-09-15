namespace Lola.UserProfile.Commands;

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
            return Result.Success();
        }

        ShowDetails(user);
        return Result.Success();
    }, "Error displaying the user profile.");

    private void ShowDetails(UserProfileEntity user) {
        Output.WriteLine("[yellow]User Information:[/]");
        Output.WriteLine($"[blue]Name:[/] {user.Name}");
        Output.WriteLine($"[blue]Preferred Language:[/] {user.Language}");
    }
}
