namespace AI.Sample.Users.Commands;

public class UserSetCommand(IHasChildren parent, IUserHandler handler)
    : Command<UserSetCommand>(parent, "Change", ["set"]) {
    protected override Task<Result> Execute(CancellationToken ct = default) {
        try {
            var user = handler.Get() ?? handler.Create();
            SetUp(user);
            handler.Set(user);

            Output.WriteLine("[green]User profile set successfully.[/]");
            Logger.LogInformation("User profile set successfully.");
            Output.WriteLine();

            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error setting the user profile.");
            Logger.LogError(ex, "Error setting the user profile.");
            Output.WriteLine();

            return Result.ErrorTask(ex.Message);
        }
    }

    private void SetUp(UserEntity user)
        => user.Name = Input.TextPrompt("How would you like me to call you?")
                                .For("name").WithDefault("Temp").AsRequired();
}
