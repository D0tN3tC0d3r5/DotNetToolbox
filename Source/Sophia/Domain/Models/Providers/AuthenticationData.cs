namespace Sophia.Models.Providers;

public class AuthenticationData {
    [Required]
    public AuthenticationType Type { get; set; }
    [Required]
    [MaxLength(2000)]
    public string? Value { get; set; }
}
