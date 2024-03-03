using DotNetToolbox.OpenAI.Chats;

namespace DotNetToolbox.Sophia.Missions;
internal record Mission {
    public required Agent Agent { get; init; }
    public required string Description { get; init; }
    public List<Message> Messages { get; } = [];
    public int TotalNumberOfTokens { get; set; }
}
