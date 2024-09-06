namespace DotNetToolbox.AI.Jobs;

public class ActionJob : IJobStrategy<Tool, bool> {
    public string Instructions => "Execute the provided tool and report the result.";

    public void AddPrompt(IChat chat, Tool input, IJobContext context) {
        var prompt = $"Execute the following tool: {input.Name}\nArguments: {string.Join(", ", input.Arguments)}";
        var message = new Message(MessageRole.User, prompt);
        chat.Messages.Add(message);
    }

    public bool GetResult(IChat chat, IJobContext context) {
        var message = chat.Messages.Last(m => m.Role == MessageRole.Assistant);
        return message.Text.Contains("success", StringComparison.CurrentCultureIgnoreCase);
    }
}
