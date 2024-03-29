namespace Sophia.WebApp.Data.Chats;

public class InstructionsEntity {
    public HashSet<string> Goals { get; set; } = [];
    public HashSet<string> Requirements { get; set; } = [];
    public HashSet<string> Assumptions { get; set; } = [];
    public HashSet<string> Constraints { get; set; } = [];
    public HashSet<string> Examples { get; set; } = [];
    public HashSet<string> Validation { get; set; } = [];

    public InstructionsData ToDto()
        => new() {
                     Goals = Goals,
                     Requirements = Requirements,
                     Assumptions = Assumptions,
                     Constraints = Constraints,
                     Examples = Examples,
                     Validation = Validation,
                 };
}
