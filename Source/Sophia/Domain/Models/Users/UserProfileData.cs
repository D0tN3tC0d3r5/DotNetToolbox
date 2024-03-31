namespace Sophia.Models.Users;

public class UserProfileData {
    [Required(AllowEmptyStrings = false)]
    [MaxLength(250)]
    public string Name { get; set; } = default!;
    [Required(AllowEmptyStrings = false)]
    [MaxLength(50)]
    public string Language { get; set; } = "English";
    public List<string> Facts { get; set; } = [];
}
