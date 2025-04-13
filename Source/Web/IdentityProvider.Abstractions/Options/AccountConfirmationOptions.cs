namespace WebApi.Options;

public record AccountConfirmationOptions {
    public bool IsRequired { get; set; }
    public AccountConfirmationType Type { get; set; }
    public TemporaryTokenOptions Token { get; set; } = new();
}
