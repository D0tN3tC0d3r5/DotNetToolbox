namespace Sophia.Models.Providers;

public class AuthorizationData {
    [Required]
    public AuthorizationType Type { get; set; }
    [Required]
    [MaxLength(2000)]
    public string? Value { get; set; }
}
