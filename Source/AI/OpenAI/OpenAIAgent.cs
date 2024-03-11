namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgent(OpenAIAgentOptions? options = null, Profile? profile = null, List<Skill>? skills = null)
    : Agent<OpenAIAgentOptions>(options ?? new(), profile ?? new(), skills ?? []);
