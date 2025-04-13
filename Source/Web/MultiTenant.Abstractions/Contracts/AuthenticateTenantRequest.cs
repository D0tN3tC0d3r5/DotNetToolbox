namespace WebApi.Contracts;

public sealed record AuthenticateTenantRequest
    : Request {
    public required string Identifier { get; init; }
    public required string Secret { get; init; }

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Identifier))
            result += new Error($"The '{nameof(Identifier)}' is required.", nameof(Identifier));
        if (string.IsNullOrWhiteSpace(Secret))
            result += new Error($"The '{nameof(Secret)}' is required.", nameof(Secret));
        return result;
    }
}