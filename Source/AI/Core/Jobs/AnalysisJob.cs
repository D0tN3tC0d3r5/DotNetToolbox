namespace DotNetToolbox.AI.Jobs;

public class AnalysisJob
    : IJobStrategy<string, string> {
    public string Instructions => "Analyze the given text and provide insights.";

    public void AddPrompt(IChat chat, string input) {
        var prompt = $"Analise the following:\n{input}";
        var message = new Message(MessageRole.User, prompt);
        chat.Messages.Add(message);
    }

    public string GetResult(IChat chat) {
        var message = chat.Messages.Last(m => m.Role == MessageRole.Assistant);
        return message.Text;
    }
}
