namespace Sophia.Models.Chats;

public class InstructionsData {
    public List<string> Goals { get; set; } = [];
    public List<string> Scope { get; set; } = [];
    public List<string> Assumptions { get; set; } = [];
    public List<string> Requirements { get; set; } = [];
    public List<string> Constraints { get; set; } = [];
    public List<string> Strategy { get; set; } = [];
    public List<string> Examples { get; set; } = [];
    public List<string> Evaluation { get; set; } = [];

    public Instructions ToModel()
        => new() {
            Goals = Goals,
            Scope = Scope,
            Requirements = Requirements,
            Assumptions = Assumptions,
            Constraints = Constraints,
            Strategy = Strategy,
            Examples = Examples,
            Validation = Evaluation,
        };
}
