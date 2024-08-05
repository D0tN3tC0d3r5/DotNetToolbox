namespace DotNetToolbox.AI.Jobs;

public class ResearchJob : IJobStrategy<string, string[]> {
    public string Instructions => "Conduct research on the given topic and provide a list of findings.";

    public void AddPrompt(IChat chat, string input) {
        var prompt = $"Research the following topic:\n{input}";
        var message = new Message(MessageRole.User, prompt);
        chat.Messages.Add(message);
    }

    public string[] GetResult(IChat chat) {
        var message = chat.Messages.Last(m => m.Role == MessageRole.Assistant);
        return message.Text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
    }
}
