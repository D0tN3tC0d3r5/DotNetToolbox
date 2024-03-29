namespace Sophia.Models.Worlds;

public class UserData {
    [MaxLength(250)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Language { get; set; } = "English";
    public HashSet<string> Facts { get; set; } = [];

    public User ToModel() => new() {
        Name = Name,
        Language = Language,
        Facts = Facts,
    };
}
