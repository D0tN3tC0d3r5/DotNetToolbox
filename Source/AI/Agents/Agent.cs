namespace DotNetToolbox.AI.Agents;

public class Agent<TOptions>(string name, TOptions options, Profile profile, List<Skill> skills)
    : IAgent
    where TOptions : class, IAgentOptions {
    public string Name { get; set; } = name;

    IAgentOptions IAgent.Options => Options;
    public TOptions Options { get; set; } = options;

    public Profile Profile { get; set; } = profile;
    public List<Skill> Skills { get; set; } = skills;
}
