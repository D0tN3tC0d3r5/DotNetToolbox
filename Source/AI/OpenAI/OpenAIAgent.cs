namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgent(string name, OpenAIAgentOptions options, Profile profile, List<Skill> skills)
    : Agent<OpenAIAgentOptions>(name, options, profile, skills);
