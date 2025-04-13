namespace WebApi.Utilities;

/// <summary>
/// Provides access to the current tenant context, typically resolved from the authenticated request.
/// </summary>
public interface ICurrentTenantAccessor {
    /// <summary>
    /// Gets the Tenant ID for the current request context, if available.
    /// </summary>
    /// <returns>The Tenant ID Guid, or null if no tenant context is established.</returns>
    Guid? GetTenantId();

    /// <summary>
    /// Gets the Tenant info from the current request context, if available.
    /// </summary>
    /// <returns>The Tenant info  or null if no tenant context is established.</returns>
    Tenant? GetTenantInfo();
}
