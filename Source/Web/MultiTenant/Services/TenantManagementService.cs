namespace WebApi.Services;

public class TenantManagementService(ITenantStore storage,
                                     IOptions<MultiTenantWebApiOptions> options,
                                     ITokenFactory tokenFactory,
                                     ILogger<TenantManagementService> logger)
    : ITenantManagementService {
    private const string _secretChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._~";
    private readonly MultiTenantWebApiOptions _options = EnsureOptionsAreValid(options.Value);

    private static MultiTenantWebApiOptions EnsureOptionsAreValid(MultiTenantWebApiOptions options) {
        var result = options.Validate();
        if (!result.HasErrors)
            return options;
        var errorMessages = string.Join("", $"{Environment.NewLine} - ${result.Errors.Select(e => e.ToString())}");
        throw new InvalidOperationException($"Invalid TenantOptions configuration:{errorMessages}");
    }

    /// <inheritdoc />
    public async Task<Result<AddTenantResponse>> RegisterAsync(AddTenantRequest request) {
        logger.LogInformation("Tenant registration requested for name '{TenantName}'.", request.Name);
        var validationResult = request.Validate();
        if (validationResult.HasErrors)
            return validationResult;

        var secret = GenerateSecret(_options.Secret.Size);
        var tenant = request.ToModel(HashSecret(secret));

        await storage.AddOrUpdateAsync(tenant);
        logger.LogInformation("Tenant registered successfully with ID '{TenantId}' for name '{TenantName}'.", tenant.Id, tenant.Name);

        return tenant.ToResponse(secret);
    }

    /// <inheritdoc />
    public async Task<Result<RenewableTokenResponse?>> AuthenticateAsync(AuthenticateTenantRequest request) {
        logger.LogInformation("Tenant authentication requested for identifier '{TenantIdentifier}'.", request.Identifier);
        var validationResult = request.Validate();
        if (validationResult.HasErrors)
            return validationResult.WithNo<RenewableTokenResponse?>();

        if (!TryDecodeTenantId(request.Identifier, out var id))
            return Result.Failure("Invalid credentials.");

        var tenant = await FindAuthenticatedTenantAsync(id, request.Secret);
        if (tenant is null)
            return Result.Failure("Invalid credentials.");

        var subject = new ClaimsIdentity([
            new(TenantClaimTypes.Identifier, Base64UrlEncoder.Encode(tenant.Id.ToByteArray())),
            new(ClaimTypes.Name, tenant.Name),
        ]);

        var accessToken = tokenFactory.CreateAccessToken(_options.TenantAccessToken, subject);

        await storage.CreateAccessTokenAsync(tenant.Id, accessToken);

        logger.LogInformation("Tenant '{TenantId}' authenticated, tokens generated.", tenant.Id);
        return accessToken.ToResponse();
    }

    /// <inheritdoc />
    public async Task<Result<RenewableTokenResponse?>> RefreshAccessTokenAsync(RenewTenantAccessRequest request) {
        logger.LogInformation("Refresh token request received.");
        var validationResult = request.Validate();
        if (validationResult.HasErrors)
            return validationResult;

        var owner = await storage.FindByActiveAccessTokenAsync(request.TenantId, request.TokenId);
        if (owner is null) {
            logger.LogWarning("Refresh token not found, expired, or already used.");
            return Result.Failure("Invalid or expired refresh token.");
        }

        await storage.CancelAccessTokenRenewalAsync(request.TokenId);

        var subject = new ClaimsIdentity([
             new(_options.Claims.Identifier, Base64UrlEncoder.Encode(owner.Id.ToByteArray())),
             new(_options.Claims.Name, owner.Name),
        ]);
        var newAccessToken = tokenFactory.CreateAccessToken(_options.TenantAccessToken, subject);

        await storage.CreateAccessTokenAsync(owner.Id, newAccessToken);
        await storage.DeleteAccessTokenAsync(request.TokenId);

        logger.LogInformation("Access token refreshed successfully for tenant '{TenantId}'. New refresh token issued.", owner.Id);
        return newAccessToken.ToResponse();
    }

    protected virtual string GenerateSecret(ushort size)
        => RandomNumberGenerator.GetString(_secretChars, size);

    private static string HashSecret(string secret)
        => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(secret)));

    private bool TryDecodeTenantId(string identifier, out Guid id) {
        id = Guid.Empty;
        try {
            var bytes = Base64UrlEncoder.DecodeBytes(identifier);
            if (bytes.Length != 16) {
                logger.LogInformation("Invalid tenant identifier: '{Identifier}'.", identifier);
                return false;
            }
            id = new(bytes);
            return true;
        }
        catch (FormatException ex) {
            logger.LogInformation(ex, "Invalid tenant identifier: '{Identifier}'.", identifier);
            return false;
        }
    }

    private async Task<Tenant?> FindAuthenticatedTenantAsync(Guid id, string secret) {
        var tenant = await storage.FindByIdAsync(id);
        if (tenant is null) {
            logger.LogInformation("Attempted to authentication non-existing Tenant with id '{TenantId}'.", id);
            return null;
        }

        if (HashSecret(secret) == tenant.Secret)
            return tenant;

        logger.LogInformation("Invalid secret provided while authenticating tenant with id '{TenantId}'.", id);
        return null;
    }
}
