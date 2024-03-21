namespace Sophia.Models.Worlds;

public class UserProfileData {
    [MaxLength(250)]
    public string Name { get; set; } = "User";

    [MaxLength(50)]
    public string Language { get; set; } = "English";

    public UserProfile ToModel() => new() {
        Name = Name,
        Language = Language,
    };
}
