namespace AI.Sample.Users.Commands;

public class UserInfoCommand : Command<UserInfoCommand> {
    private readonly IUserHandler _handler;

    public UserInfoCommand(IHasChildren parent, IUserHandler handler)
        : base(parent, "Info", ["i"]) {
        _handler = handler;
        Description = "Display the User Profile.";
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var user = _handler.Get();
        if (user is null) {
            Logger.LogInformation("No user selected.");
            Output.WriteLine();

            return Result.SuccessTask();
        }

        Output.WriteLine($"[yellow]User Information:[/]");
        Output.WriteLine($"[blue]Id:[/] {user.Key}");
        Output.WriteLine($"[blue]Name:[/] {user.Name}");
        Output.WriteLine();

        return Result.SuccessTask();
    }
}
