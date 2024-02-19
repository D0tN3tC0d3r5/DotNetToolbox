namespace Sophia;

public class SophiaShellApplication : ShellApplication<SophiaShellApplication> {
    private readonly IChatHandler _chatHandler;

    public SophiaShellApplication(string[] args, IServiceProvider services, IChatHandler chatHandler)
        : base(args, services) {
        _chatHandler = chatHandler;
        AddCommand<StartChatCommand>();
        AddCommand<EndChatCommand>();
        AddCommand<SendMessageCommand>();
    }

    protected override async Task<Result> ExecuteDefault(string input, CancellationToken ct) {
        var chatId = Context.GetValueOrDefault("CurrentChatId");
        if (string.IsNullOrEmpty(chatId)) {
            var chat = await _chatHandler.Create("gpt-4-0125-preview");
            Context["CurrentChatId"] = chatId = chat.Id;
            Environment.Output.WriteLine($"Chat session '{chatId}' started.");
        }

        var response = await _chatHandler.SendMessage(chatId, input);
        Environment.Output.WriteLine($"- {response}");
        return Result.Success();
    }
}
