namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    IAgentOptions Options { get; }
    Profile Profile { get; set; }
    List<Skill> Skills { get; set; }
}
