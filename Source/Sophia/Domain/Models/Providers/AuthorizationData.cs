namespace Sophia.Models.Providers;

public class AuthorizationData {
    [Required]
    public AuthorizationType Type { get; init; }
    [Required]
    public AuthorizationScheme Scheme { get; init; }
    [Required]
    [MaxLength(2000)]
    public string? Value { get; set; }
    public DateTimeOffset? ExpiresOn { get; set; }
}
