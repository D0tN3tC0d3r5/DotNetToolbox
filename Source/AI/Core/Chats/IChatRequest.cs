namespace DotNetToolbox.AI.Chats;

public interface IChatRequest {
    string Model { get; }
    string Map { get; }
    IEnumerable<IChatRequestMessage> Messages { get; }
    uint MaximumOutputTokens { get; }
}
