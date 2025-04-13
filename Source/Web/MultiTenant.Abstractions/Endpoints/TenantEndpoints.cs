namespace WebApi.Endpoints;

public static class TenantEndpoints {
    public const string TenantsPrefix = "/tenants";
    public const string Register = "/";
    public static class Authentication {
        public const string AuthenticationPrefix = "/auth";
        public const string Authenticate = "/";
        public const string Refresh = "/";
    }
}
