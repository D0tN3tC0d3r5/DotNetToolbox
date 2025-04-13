using static Microsoft.AspNetCore.Http.StatusCodes;
using static WebApi.Endpoints.IdentityApiPaths.Authentication;
using static WebApi.Endpoints.UserEndpointHandlers;

// ReSharper disable once CheckNamespace
namespace WebApi.Endpoints;

public static class UserEndpointMappings {
    public static IEndpointRouteBuilder MapUserAuthenticationEndpoints(this IEndpointRouteBuilder app) {
        var group = app.MapGroup(AuthenticationPrefix).WithTags("User Authentication");

        group.MapPost(SignIn, SignInAsync)
             .WithName("SignIn")
             .Produces<SignInResponse>()
             .Produces<HttpValidationProblemDetails>(Status400BadRequest)
             .Produces<SignInResponse>(Status401Unauthorized)
             .Produces<SignInResponse>(Status403Forbidden)
             .Produces<SignInResponse>(Status404NotFound)
             .Produces<ProblemDetails>(Status500InternalServerError);

        group.MapPost(SignOut, SignOutAsync)
             .WithName("SignOut")
             .Produces(Status200OK)
             .Produces<HttpValidationProblemDetails>(Status400BadRequest)
             .Produces<ProblemDetails>(Status500InternalServerError);

        group.MapGet(GetAuthenticationSchemes, GetAuthenticationSchemesAsync)
             .WithName("GetAuthenticationSchemes")
             .Produces<AuthenticationScheme[]>()
             .Produces<ProblemDetails>(Status500InternalServerError);

        return app;
    }
}
