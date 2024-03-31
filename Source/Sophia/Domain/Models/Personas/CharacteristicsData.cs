namespace Sophia.Models.Personas;

public class CharacteristicsData {
    public List<string> Cognition { get; set; } = [];
    public List<string> Disposition { get; set; } = [];
    public List<string> Interaction { get; set; } = [];
    public List<string> Attitude { get; set; } = [];

    public Characteristics ToModel()
        => new() {
            Cognition = Cognition,
            Disposition = Disposition,
            Interaction = Interaction,
            Attitude = Attitude,
        };
}
