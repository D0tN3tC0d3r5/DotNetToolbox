namespace Sophia.Models.Users;

public class UserData
    : ISimpleKeyEntity<UserData, string> {
    [Required(AllowEmptyStrings = false)]
    [MaxLength(36)]
    public string Id { get; set; } = default!;
    [Required(AllowEmptyStrings = false)]
    [MaxLength(250)]
    public string Name { get; set; } = default!;
    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string Language { get; set; } = "English";
    public List<string> Facts { get; set; } = [];

    public UserProfile ToModel() => new() {
        Id = Id,
        Name = Name,
        Language = Language,
        Facts = Facts,
    };
}
