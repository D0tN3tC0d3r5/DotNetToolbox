namespace DotNetToolbox.Sophia.Commands;

public class EndChatCommand : Command<EndChatCommand> {
    public EndChatCommand(IHasChildren parent)
        : base(parent, "End-Chat", ["ec"]) {
        Description = "Terminates the current chat session after confirmation.";
    }

    public override Task<Result> Execute(CancellationToken ct = default) {
        var chat = Application.Context.GetValueOrDefault("CurrentChat");
        if (chat is null) {
            Environment.Output.WriteLine("No active chat session to end.");
            return Result.SuccessTask();
        }

        var isYes = Ask.YesOrNo("Are you sure you want to end this chat session?");
        if (!isYes) return Result.SuccessTask();

        Application.Context.Remove("CurrentChatId");
        Environment.Output.WriteLine("Chat session ended.");
        return Result.SuccessTask();
    }
}
