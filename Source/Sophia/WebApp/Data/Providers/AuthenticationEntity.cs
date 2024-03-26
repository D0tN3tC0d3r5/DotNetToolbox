namespace Sophia.WebApp.Data.Providers;

public class AuthenticationEntity {
    [Required]
    public AuthenticationType Type { get; init; }
    [MaxLength(int.MaxValue)]
    public string? Value { get; set; }

    public AuthenticationData ToDto()
        => new() {
            Type = Type,
            Value = Value,
        };
}
