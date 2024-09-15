using Task = System.Threading.Tasks.Task;
using ValidationException = DotNetToolbox.Results.ValidationException;

namespace AI.Sample.UserProfile.Commands;

public class UserProfileSetCommand(IHasChildren parent, IUserProfileHandler handler)
    : Command<UserProfileSetCommand>(parent, "Change", n => {
        n.Aliases = ["set"];
        n.Description = "Update your profile.";
        n.Description = "Change the current user profile.";
    }) {
    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        try {
            Logger.LogInformation("Executing UserProfile->Set command...");
            var user = handler.CurrentUser ?? handler.Create();
            await SetUpAsync(user, lt);
            handler.Set(user);

            Output.WriteLine("[green]User profile set successfully.[/]");
            Logger.LogInformation("User profile set successfully.");
            return Result.Success();
        } catch (ValidationException ex) {
            var errors = string.Join("\n", ex.Errors.Select(e => $" - {e.Source}: {e.Message}"));
            Logger.LogWarning("Error setting the your user profile. Validation errors:\n{Errors}", errors);
            Output.WriteLine($"[red]We found some problems while setting your user profile. Please correct the following errors and try again:\n{errors}[/]");
            return Result.Invalid(ex.Errors);
        }
    }, "Error setting the user profile.", ct);

    private async Task SetUpAsync(UserProfileEntity user, CancellationToken ct)
        => user.Name = await Input.BuildTextPrompt<string>("How would you like me to call you?")
                                  .Validate(UserProfileEntity.ValidateName)
                                  .ShowAsync(ct);
}
