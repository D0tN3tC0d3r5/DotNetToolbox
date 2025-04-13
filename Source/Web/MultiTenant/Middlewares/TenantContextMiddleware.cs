namespace WebApi.Middlewares;

internal sealed class TenantContextMiddleware(RequestDelegate next, ITenantStore tenantStore, IOptions<MultiTenantWebApiOptions> options, ILogger<TenantContextMiddleware> logger) {
    private readonly MultiTenantWebApiOptions _options = options.Value;
    internal const string TenantIdKey = "TenantId";
    internal const string TenantInfoKey = "TenantInfo";

    public async Task InvokeAsync(HttpContext context) {
        if (!await TryValidateTenant(context))
            logger.LogInformation("Tenant validation failed.");
        await next(context);
    }

    private async Task<bool> TryValidateTenant(HttpContext context) {
        if (!context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader)) {
            logger.LogDebug("Authorization header not found, skipping tenant context setup.");
            return false;
        }

        var tokenParts = authorizationHeader.ToString().Split(" ");
        if (tokenParts is not ["Bearer", var token]) {
            logger.LogDebug("Authorization header is not in the correct format, skipping tenant context setup.");
            return false;
        }

        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token)) {
            logger.LogDebug("Invalid JWT token, skipping tenant context setup.");
            return false;
        }

        var jwtToken = handler.ReadJwtToken(token);
        var tenantIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == _options.Claims.Identifier)?.Value;
        if (string.IsNullOrWhiteSpace(tenantIdClaim)) {
            logger.LogDebug("Tenant identifier claim ('{ClaimType}') not found or empty.", _options.Claims.Identifier);
            return false;
        }

        if (!TryDecodeIdentifierClaim(tenantIdClaim, out var tenantId))
            return false;

        var tenant = await tenantStore.FindByIdAsync(tenantId, context.RequestAborted);
        if (tenant is null) {
            logger.LogDebug("Tenant not found: {TenantIdentifier}.", tenantId);
            return false;
        }

        context.Items[TenantIdKey] = tenantId;
        context.Items[TenantInfoKey] = tenant;
        return true;
    }

    private bool TryDecodeIdentifierClaim(string claimValue, out Guid id) {
        id = Guid.Empty;
        try {
            var bytes = Base64UrlEncoder.DecodeBytes(claimValue);

            if (bytes.Length != 16) {
                logger.LogDebug("Tenant identifier claim found but has invalid length: {IdentifierClaim}", claimValue);
                return false;
            }

            id = new(bytes);
            return true;
        }
        catch (Exception ex) {
            logger.LogDebug(ex, "Failed to decode tenant identifier claim: {IdentifierClaim}", claimValue);
            return false;
        }
    }
}
