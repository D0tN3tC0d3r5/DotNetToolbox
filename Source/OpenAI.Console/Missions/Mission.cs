using DotNetToolbox.OpenAI.Agents;

using Agent = DotNetToolbox.Sophia.Agents.Agent;

namespace DotNetToolbox.Sophia.Missions;
internal record Mission {
    public required Agent Agent { get; init; }
    public required string Description { get; init; }
    public List<Message> Messages { get; } = [];
    public int TotalNumberOfTokens { get; set; }
}
