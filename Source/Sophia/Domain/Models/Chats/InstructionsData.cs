namespace Sophia.Models.Chats;

public class InstructionsData {
    public HashSet<string> Goals { get; set; } = [];
    public HashSet<string> Requirements { get; set; } = [];
    public HashSet<string> Assumptions { get; set; } = [];
    public HashSet<string> Constraints { get; set; } = [];
    public HashSet<string> Examples { get; set; } = [];
    public HashSet<string> Validation { get; set; } = [];

    public Instructions ToModel()
        => new() {
            Goals = Goals,
            Requirements = Requirements,
            Assumptions = Assumptions,
            Constraints = Constraints,
            Examples = Examples,
            Validation = Validation,
        };
}
