namespace DotNetToolbox.AI.Agents;

public interface IChatRequest {
    string Model { get; }
    string Map { get; }
    IEnumerable<IChatRequestMessage> Messages { get; }
    uint MaximumOutputTokens { get; }
}
