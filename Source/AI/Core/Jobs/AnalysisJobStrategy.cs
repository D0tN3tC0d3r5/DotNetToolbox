namespace DotNetToolbox.AI.Jobs;

public class AnalysisJobStrategy
    : IJobStrategy<string, string> {
    public string Instructions => "Analyze the given text and provide insights.";

    public void AddPrompt(IChat chat, string input, IJobContext context) {
        var prompt = $"Analise the following:\n{input}";
        var message = new Message(MessageRole.User, prompt);
        chat.Add(message);
    }

    public string GetResult(IChat chat, IJobContext context) {
        var message = chat.Last(m => m.Role == MessageRole.Assistant);
        return message.Text;
    }
}

public class AnalysisJob(string id, JobContext context)
    : Job<string, string>(id, context);
