namespace WebApi.Contracts;

public record AddTenantRequest
    : Request {
    public required string Name { get; set; }

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name))
            result += new Error($"The '{nameof(Name)}' is required.", nameof(Name));
        return result;
    }
}