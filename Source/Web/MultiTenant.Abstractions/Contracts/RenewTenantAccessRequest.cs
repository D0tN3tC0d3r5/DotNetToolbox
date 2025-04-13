namespace WebApi.Contracts;

public sealed record RenewTenantAccessRequest
    : Request {
    public required Guid TenantId { get; init; }
    public required Guid TokenId { get; init; }
}
