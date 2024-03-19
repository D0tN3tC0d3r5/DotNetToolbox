namespace Sophia.WebApp.Data.World;

[Owned]
public class UserProfileEntity {
    [MaxLength(250)]
    public string? Name { get; set; }
    [MaxLength(50)]
    public string? Language { get; set; }

    public UserProfileData ToDto() => new() {
        Name = Name,
        Language = Language,
    };
}
