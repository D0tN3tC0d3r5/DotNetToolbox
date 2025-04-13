namespace WebApi.Contracts;

public sealed record AddTenantResponse
    : Response {
    public required string Id { get; init; }
    public required string Secret { get; init; }
}