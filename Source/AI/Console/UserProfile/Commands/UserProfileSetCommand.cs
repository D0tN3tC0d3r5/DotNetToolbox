namespace AI.Sample.UserProfile.Commands;

public class UserProfileSetCommand(IHasChildren parent, IUserProfileHandler handler)
    : Command<UserProfileSetCommand>(parent, "Change", ["set"]) {
    protected override Result Execute() {
        try {
            var user = handler.Get() ?? handler.Create();
            SetUp(user);
            handler.Set(user);

            Output.WriteLine("[green]User profile set successfully.[/]");
            Logger.LogInformation("User profile set successfully.");
            Output.WriteLine();

            return Result.Success();
        }
        catch (Exception ex) {
            Output.WriteError("Error setting the user profile.");
            Logger.LogError(ex, "Error setting the user profile.");
            Output.WriteLine();

            return Result.Error(ex.Message);
        }
    }

    private void SetUp(UserProfileEntity user)
        => user.Name = Input.BuildTextPrompt<string>("How would you like me to call you?")
                                .For("name").WithDefault("Temp").AsRequired();
}
