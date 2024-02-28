namespace DotNetToolbox.OpenAI.Agents;
public class Agent {
    public Agent(AgentOptions? options = null) {
        Options = options ?? Options;
    }

    public string Id { get; init; } = Guid.NewGuid().ToString();
    public List<Message> Messages { get; } = [];
    public AgentOptions Options { get; } = new();
    public int TotalNumberOfTokens { get; set; }
}
