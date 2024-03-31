namespace Sophia.Models.Users;

public class ApplicationUserProfile
    : User {
    [Key]
    [MaxLength(450)]
    public string Id { get; set; } = default!;

    public UserData ToDto() => new() {
        Name = Name,
        Language = Language,
    };
}
