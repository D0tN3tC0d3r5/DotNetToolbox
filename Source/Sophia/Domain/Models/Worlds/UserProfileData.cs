namespace Sophia.Models.Worlds;

public class UserProfileData {
    [MaxLength(250)]
    public string? Name { get; set; }
    [MaxLength(50)]
    public string? Language { get; set; }

    public UserProfile ToModel() => new() {
        Name = Name,
        Language = Language,
    };
}
