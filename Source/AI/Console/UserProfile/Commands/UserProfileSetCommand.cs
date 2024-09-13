using Task = System.Threading.Tasks.Task;

namespace AI.Sample.UserProfile.Commands;

public class UserProfileSetCommand(IHasChildren parent, IUserProfileHandler handler)
    : Command<UserProfileSetCommand>(parent, "Change", ["set"]) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async (ct) => {
        var user = handler.CurrentUser ?? handler.Create();
        await SetUpAsync(user, ct);
        handler.Set(user);

        Output.WriteLine("[green]User profile set successfully.[/]");
        Logger.LogInformation("User profile set successfully.");
        Output.WriteLine();

        return Result.Success();
    }, "Error setting the user profile.", ct);

    private async Task SetUpAsync(UserProfileEntity user, CancellationToken ct)
        => user.Name = await Input.BuildTextPrompt<string>("How would you like me to call you?")
                            .For("name").WithDefault("Temp")
                            .AsRequired()
                            .ShowAsync(ct);

}
