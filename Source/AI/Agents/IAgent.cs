namespace DotNetToolbox.AI.Agents;

public interface IAgent {
    string Name { get; }
    IAgentOptions Options { get; }
    Profile Profile { get; set; }
    List<Skill> Skills { get; set; }
}
