namespace WebApi.Services;

/// <summary>
/// Defines the contract for managing tenants and their authentication tokens.
/// </summary>
public interface ITenantManagementService {
    /// <summary>
    /// Registers a new tenant.
    /// </summary>
    /// <param name="request">The tenant registration request details.</param>
    /// <returns>A result containing the response data for the newly registered tenant, or errors if registration failed.</returns>
    Task<Result<AddTenantResponse>> RegisterAsync(AddTenantRequest request);

    /// <summary>
    /// Authenticates a tenant using identifier/secret and returns access/refresh tokens.
    /// </summary>
    /// <param name="request">The tenant authentication request details.</param>
    /// <returns>A result containing the token response on success, null if authentication failed, or errors.</returns>
    Task<Result<RenewableTokenResponse?>> AuthenticateAsync(AuthenticateTenantRequest request);

    /// <summary>
    /// Refreshes an access token using a valid refresh token.
    /// Implements refresh token rotation (issues a new refresh token).
    /// </summary>
    /// <param name="request">The request containing the refresh token.</param>
    /// <returns>A result containing a new RenewableTokenResponse on success, null if refresh failed, or errors.</returns>
    Task<Result<RenewableTokenResponse?>> RefreshAccessTokenAsync(RenewTenantAccessRequest request);
}
