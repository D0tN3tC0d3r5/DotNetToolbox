namespace DotNetToolbox.AI.Anthropic;

public class AnthropicAgent(AnthropicAgentOptions? options = null, Profile? profile = null, List<Skill>? skills = null)
    : Agent<AnthropicAgentOptions>(options ?? new(), profile ?? new(), skills ?? []);
