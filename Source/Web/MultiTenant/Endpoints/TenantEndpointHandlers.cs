using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebApi.Endpoints;

internal static class TenantEndpointHandlers
{
    internal static async Task<IResult> RegisterAsync(
        [FromServices] ITenantManagementService service,
        [FromBody] AddTenantRequest request,
        [FromServices] ILoggerFactory loggerFactory) {
        var logger = loggerFactory.CreateLogger($"{nameof(TenantEndpointHandlers)}.{nameof(RegisterAsync)}");
        try {
            var result = await service.RegisterAsync(request);
            return result.HasErrors
                       ? Results.BadRequest(result.Errors)
                       : Results.Ok(result.Value);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Tenant registration failed for request: {Request}.", request);
            return Results.Problem("An unexpected error occurred during the registration.",
                                   statusCode: StatusCodes.Status500InternalServerError,
                                   title: "Registration Failed");
        }
    }

    internal static async Task<IResult> AuthenticateAsync(
        [FromBody] AuthenticateTenantRequest request,
        [FromServices] ITenantManagementService service,
        [FromServices] ILoggerFactory loggerFactory) {
        var logger = loggerFactory.CreateLogger($"{nameof(TenantEndpointHandlers)}.{nameof(AuthenticateAsync)}");
        var result = await service.AuthenticateAsync(request);
        try {
            return result.HasErrors
                       ? Results.BadRequest(result.Errors)
                       : result.Value is null ? Results.Unauthorized()
                           : Results.Ok(result.Value);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Tenant authentication failed for tenant: {TenantIdentifier}.", request.Identifier);
            return Results.Problem("An unexpected error occurred during the tenant authentication.",
                                   statusCode: StatusCodes.Status500InternalServerError,
                                   title: "Tenant Authentication Failed");
        }
    }

    internal static async Task<IResult> RenewAuthenticationAsync(
        // Expect refresh token, e.g., in the body
        [FromBody] RenewTenantAccessRequest request, // Define this simple request record
        [FromServices] ITenantManagementService service,
        [FromServices] ILoggerFactory loggerFactory) {
        var logger = loggerFactory.CreateLogger($"{nameof(TenantEndpointHandlers)}.{nameof(RenewAuthenticationAsync)}");
        try {
            var result = await service.RefreshAccessTokenAsync(request);
            return result.HasErrors
                       ? Results.BadRequest(result.Errors)
                       : result.Value is null ? Results.Unauthorized()
                           : Results.Ok(result.Value);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Tenant access token refresh failed for token: {TokenId}.", request.TokenId);
            return Results.Problem("An unexpected error occurred during the access token refresh.",
                                   statusCode: StatusCodes.Status500InternalServerError,
                                   title: "Access Token Refresh Failed");
        }
    }
}
