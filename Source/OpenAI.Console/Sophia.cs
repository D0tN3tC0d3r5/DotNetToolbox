namespace DotNetToolbox.Sophia;

public class Sophia : ShellApplication<Sophia> {
    private readonly IChatHandler _chatHandler;

    public Sophia(string[] args, IServiceProvider services, IChatHandler chatHandler)
        : base(args, services) {
        _chatHandler = chatHandler;
        AddCommand<StartChatCommand>();
        AddCommand<EndChatCommand>();
        AddCommand<SendMessageCommand>();
    }

    protected override async Task<Result> ExecuteDefault(string input, CancellationToken ct) {
        var chatId = Context.GetValueOrDefault("CurrentChatId");
        if (string.IsNullOrEmpty(chatId)) {
            var chat = await _chatHandler.Create().ConfigureAwait(false);
            Context["CurrentChatId"] = chatId = chat.Id;
            Environment.Output.WriteLine($"Chat session '{chatId}' started.");
        }

        Environment.Output.Write("- ");
        await _chatHandler.SendMessage(chatId, input, c => Task.Run(() => Environment.Output.Write(c), ct)).ConfigureAwait(false);
        Environment.Output.WriteLine();
        return Result.Success();
    }
}
