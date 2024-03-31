namespace Sophia.Data.Chats;

public class InstructionsEntity {
    public List<string> Goals { get; set; } = [];
    public List<string> Scope { get; set; } = [];
    public List<string> Requirements { get; set; } = [];
    public List<string> Assumptions { get; set; } = [];
    public List<string> Constraints { get; set; } = [];
    public List<string> Examples { get; set; } = [];
    public List<string> Strategy { get; set; } = [];
    public List<string> Validation { get; set; } = [];
}
