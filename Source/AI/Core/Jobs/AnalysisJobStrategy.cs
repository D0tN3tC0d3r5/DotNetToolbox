namespace DotNetToolbox.AI.Jobs;

public class AnalysisJobStrategy
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

public class AnalysisJob(string id, Context context, IAgent agent)
    : Job<string, string>(new AnalysisJobStrategy(), id, context, agent);
