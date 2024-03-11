namespace DotNetToolbox.AI.Agents;

public class Agent<TOptions>(TOptions? options = null, Profile? profile = null, List<Skill>? skills = null)
    : IAgent
    where TOptions : class, IAgentOptions, new() {
    IAgentOptions IAgent.Options => Options;
    public TOptions Options { get; set; } = IsValidOrDefault(options, new());
    public Profile Profile { get; set; } = profile ?? new();
    public List<Skill> Skills { get; set; } = skills ?? [];
}
