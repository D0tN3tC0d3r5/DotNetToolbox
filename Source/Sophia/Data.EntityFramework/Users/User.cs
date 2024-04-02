namespace Sophia.Data.Users;

public class User
    : IdentityUser,
      ISimpleKeyEntity<User, string>  {
    [MaxLength(250)]
    [ProtectedPersonalData]
    [Required(AllowEmptyStrings = false)]
    public string Name { get; set; } = default!;
    [PersonalData]
    [MaxLength(50)]
    [Required(AllowEmptyStrings = false)]
    public string Language { get; set; } = "English";
    [ProtectedPersonalData]
    public List<string> Facts { get; set; } = [];
}

