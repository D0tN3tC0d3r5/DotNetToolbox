namespace Sophia.Commands;

public class SendMessageCommand : Command<SendMessageCommand> {
    private readonly IChatHandler _chatHandler;

    public SendMessageCommand(IHasChildren parent, IChatHandler chatHandler)
        : base(parent, "Send", ["s"]) {
        _chatHandler = chatHandler;
        Description = "Sends a message to the current chat session and displays the response.";
        AddParameter("message", "The message to send to the OpenAI chat.");
    }

    public override async Task<Result> Execute(CancellationToken ct = default) {
        var chatId = Application.Context.GetValueOrDefault("CurrentChatId");
        if (string.IsNullOrEmpty(chatId)) {
            Environment.Output.WriteLine("No active chat session. Please start a chat first.");
            return Result.Invalid("No active chat session.");
        }

        var message = Context["message"];
        var response = await _chatHandler.SendMessage(chatId, message!);
        Environment.Output.WriteLine($"Response: {response}");

        return Result.Success();
    }
}
