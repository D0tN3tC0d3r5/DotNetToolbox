namespace WebApi.Options;

public record MasterIdentityOptions {
    public MasterIdentityOptions() {
        Identifier = Email;
    }
    public string Identifier { get; init; }
    public string Email { get; init; } = "master@host.com";
    public string PhoneNumber { get; init; } = string.Empty;
    public string? HashedSecret { get; init; }
}
