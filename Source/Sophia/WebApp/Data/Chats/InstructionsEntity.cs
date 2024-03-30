namespace Sophia.WebApp.Data.Chats;

public class InstructionsEntity {
    public List<string> Goals { get; set; } = [];
    public List<string> Scope { get; set; } = [];
    public List<string> Requirements { get; set; } = [];
    public List<string> Assumptions { get; set; } = [];
    public List<string> Constraints { get; set; } = [];
    public List<string> Examples { get; set; } = [];
    public List<string> Strategy { get; set; } = [];
    public List<string> Validation { get; set; } = [];

    public InstructionsData ToDto()
        => new() {
            Goals = Goals,
            Scope = Scope,
            Requirements = Requirements,
            Assumptions = Assumptions,
            Constraints = Constraints,
            Examples = Examples,
            Strategy = Strategy,
            Evaluation = Validation,
        };
}
