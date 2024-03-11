namespace DotNetToolbox.AI.Anthropic;

public class AnthropicAgent(string name, AnthropicAgentOptions options, Profile profile, List<Skill> skills)
    : Agent<AnthropicAgentOptions>(name, options, profile, skills);
