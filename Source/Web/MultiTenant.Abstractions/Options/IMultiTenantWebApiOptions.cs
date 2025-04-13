namespace WebApi.Options;

public interface IMultiTenantWebApiOptions
    : IMultiTenantWebApiOptions<IMultiTenantWebApiOptions>;

public interface IMultiTenantWebApiOptions<out TOptions>
    : IWebApiOptions<TOptions>
    where TOptions : IMultiTenantWebApiOptions<TOptions> {
    TenantClaimsOptions Claims { get; set; }
    SecretOptions Secret { get; set; }
    AccessTokenOptions TenantAccessToken { get; }
}
