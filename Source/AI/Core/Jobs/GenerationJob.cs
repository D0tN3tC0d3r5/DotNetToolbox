namespace DotNetToolbox.AI.Jobs;

public class GenerationJob : IJobStrategy<string, byte[]> {
    public string Instructions => "Generate content based on the given description.";

    public void AddPrompt(IChat chat, string input, IJobContext context) {
        var prompt = $"Generate the following:\n{input}";
        var message = new Message(MessageRole.User, prompt);
        chat.Messages.Add(message);
    }

    public byte[] GetResult(IChat chat, IJobContext context) {
        var message = chat.Messages.Last(m => m.Role == MessageRole.Assistant);
        return Convert.FromBase64String(message.Text);
    }
}
