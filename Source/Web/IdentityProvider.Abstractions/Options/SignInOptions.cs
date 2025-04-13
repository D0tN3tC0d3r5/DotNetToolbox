namespace WebApi.Options;

public record SignInOptions {
    public SignInType Type { get; set; }
    public LockoutOptions Lockout { get; set; } = new();
    public PasswordOptions Password { get; set; } = new();
}
