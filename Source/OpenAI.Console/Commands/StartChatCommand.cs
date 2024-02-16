namespace Lola.Commands;

public class StartChatCommand : Command<StartChatCommand> {
    private readonly IChatHandler _chatHandler;

    public StartChatCommand(IHasChildren parent, IChatHandler chatHandler)
        : base(parent, "Start-Chat", ["sc"]) {
        _chatHandler = chatHandler;
        Description = "Starts a new chat session with OpenAI.";
    }

    public override async Task<Result> Execute(CancellationToken ct = default) {
        if (Application.Context.TryGetValue("CurrentChatId", out var chatId)) {
            Environment.Output.WriteLine($"There is already an active chat session: '{chatId}'.");
            return Result.Success();
        }

        var chat = await _chatHandler.Create("gpt-4-0125-preview");
        Application.Context["CurrentChatId"] = chat.Id;
        Environment.Output.WriteLine($"Chat session '{chat.Id}' started.");
        return Result.Success();
    }
}
