namespace DotNetToolbox.OpenAI.Agents;
public class Chat {
    public Chat(ChatOptions? options = null) {
        Options = options ?? Options;
    }

    public string Id { get; init; } = Guid.NewGuid().ToString();
    public List<Message> Messages { get; } = [];
    public ChatOptions Options { get; } = new();
    public int TotalNumberOfTokens { get; set; }
}
