namespace WebApi.Extensions;

/// <summary>
/// Extension methods for adding the TenantContextMiddleware to the pipeline.
/// </summary>
public static class ApplicationBuilderExtensions {
    /// <summary>
    /// Adds the TenantContextMiddleware to the request pipeline.
    /// This should be placed *after* authentication and authorization middleware.
    /// </summary>
    /// <param name="builder">The application builder.</param>
    /// <returns>The application builder.</returns>
    public static IApplicationBuilder UseTenantContext(this IApplicationBuilder builder) {
        ArgumentNullException.ThrowIfNull(builder);
        return builder.UseMiddleware<TenantContextMiddleware>();
    }
}