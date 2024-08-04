namespace DotNetToolbox.AI.Jobs;

public class AnalysisJob
    : IJobStrategy<string, string> {
    public string Instructions => "Analyze the given text and provide insights.";

    public IChat PrepareChat(JobContext context, string input) {
        var chat = new Chat();
        chat.Messages.Add(new Message(MessageRole.System, $"{context}"));
        chat.Messages.Add(new Message(MessageRole.User, input));
        return chat;
    }

    public string GetResult(IChat chat) {
        var assistantMessage = chat.Messages.Last(m => m.Role == MessageRole.Assistant);
        return assistantMessage.Text;
    }
}
