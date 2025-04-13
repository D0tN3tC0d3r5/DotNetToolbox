using static WebApi.Endpoints.TenantEndpointHandlers;
using static WebApi.Endpoints.TenantEndpoints;
using static WebApi.Endpoints.TenantEndpoints.Authentication;

// ReSharper disable once CheckNamespace
namespace WebApi.Endpoints;

public static class TenantEndpointMappings {
    private static RouteGroupBuilder? _tenantsGroup;
    private static RouteGroupBuilder GetTenantsGroup(IEndpointRouteBuilder app)
        => _tenantsGroup ??= app.MapGroup(TenantsPrefix).WithTags("Tenants");

    public static IEndpointRouteBuilder MapTenantManagementEndpoints(this IEndpointRouteBuilder app) {
        GetTenantsGroup(app).MapPost(Register, RegisterAsync)
             .WithName("RegisterTenant")
             .Produces<AddTenantResponse>()
             .Produces(StatusCodes.Status400BadRequest)
             .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        return app;
    }

    public static IEndpointRouteBuilder MapTenantAuthenticationEndpoints(this IEndpointRouteBuilder app) {
        var authGroup = GetTenantsGroup(app).MapGroup(AuthenticationPrefix) // Changed group to /auth
                           .WithTags("Tenant Authentication")
                           .WithDescription("Endpoints for tenant authentication and token management.");

        authGroup.MapPost(Authenticate, AuthenticateAsync) // New endpoint for standard auth
             .WithName("AuthenticateTenant")
             .WithSummary("Authenticates a tenant using client ID/secret and returns access/refresh tokens.")
             .Produces<RenewableTokenResponse>()
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
             .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized)
             .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        authGroup.MapPost(Refresh, RenewAuthenticationAsync) // New endpoint for refresh
             .WithName("RefreshTenantToken")
             .WithSummary("Refreshes an access token using a refresh token.")
             .Produces<RenewableTokenResponse>()
             .Produces<ProblemDetails>(StatusCodes.Status400BadRequest) // e.g., missing refresh token
             .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized); // e.g., invalid/expired refresh token

        return app;
    }
}