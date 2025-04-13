namespace WebApi.Options;

public record JsonWebTokenOptions
    : TemporaryTokenOptions {
    public const ushort MinimumKeySize = 32;

    public JsonWebTokenOptions() {
    }

    protected JsonWebTokenOptions(uint lifetimeInSeconds)
        : base(lifetimeInSeconds) {
    }

    [Required]
    public string Key { get; set; } = null!;
    public string? Issuer { get; set; }
    public string? Audience { get; set; }

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Key))
            result += "The JWT token key is required.";
        if (string.IsNullOrWhiteSpace(Issuer))
            result += "The JWT token issuer is required.";
        if (string.IsNullOrWhiteSpace(Audience))
            result += "The JWT token audience is required.";

        try {
            var keyBytes = Convert.FromBase64String(Key);
            if (keyBytes.Length < MinimumKeySize) // 256 bits or 32 bytes
                result += "The JWT token key size is insufficient for HMAC-SHA256 (requires at least 256 bits / 32 bytes).";
        }
        catch {
            result += "The JWT token key is not a valid base64 string.";
        }

        return result;
    }
}