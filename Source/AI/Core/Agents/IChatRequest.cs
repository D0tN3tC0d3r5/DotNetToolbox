namespace DotNetToolbox.AI.Agents;

public interface IChatRequest {
    string Model { get; }
    string Context { get; }
    IEnumerable<IChatRequestMessage> Messages { get; }
    uint MaximumOutputTokens { get; }
}
