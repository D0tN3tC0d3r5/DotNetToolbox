namespace WebApi.Options;

public record MultiTenantWebApiOptions
    : MultiTenantWebApiOptions<MultiTenantWebApiOptions>
    , IMultiTenantWebApiOptions;

public record MultiTenantWebApiOptions<TOptions>
    : WebApiOptions<TOptions>
    , IMultiTenantWebApiOptions<TOptions>
    where TOptions : MultiTenantWebApiOptions<TOptions>, new() {
    public TenantClaimsOptions Claims { get; set; } = new();
    public SecretOptions Secret { get; set; } = new();
    public AccessTokenOptions TenantAccessToken { get; set; } = new();

    public override Result Validate(IMap? context = null) {
        var result = base.Validate(context);
        result += Secret.Validate(context);
        result += TenantAccessToken.Validate(context);
        return result;
    }
}