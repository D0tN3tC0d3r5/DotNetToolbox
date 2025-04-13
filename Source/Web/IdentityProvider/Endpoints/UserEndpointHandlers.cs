using static WebApi.Model.SignInResult;

using IAuthenticationService = WebApi.Services.IAuthenticationService;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace WebApi.Endpoints;

internal static class UserEndpointHandlers
{
    internal static async Task<IResult> SignInAsync(
        [FromServices] IAuthenticationService service,
        [FromBody] PasswordSignInRequest request,
        [FromServices] ILoggerFactory loggerFactory) {
        var logger = loggerFactory.CreateLogger($"{nameof(UserEndpointMappings.MapUserAuthenticationEndpoints)}.{nameof(SignInAsync)}");
        try {
            var result = await service.PasswordSignIn(request);
            return result.Status switch {
                       InvalidInput => Results.ValidationProblem(result.Errors.GroupedBySource(), "The sign-in request contains one or more errors.", title: "Invalid Sign-In Request"),
                       AccountNotFound => Results.NotFound(new SignInResponse(result.Status)),
                       SignInIsNotAllowed => Results.Json(new SignInResponse(result.Status), statusCode: StatusCodes.Status401Unauthorized),
                       AccountIsBlocked => Results.Json(new SignInResponse(result.Status), statusCode: StatusCodes.Status401Unauthorized),
                       AccountIsLocked => Results.Json(new SignInResponse(result.Status), statusCode: StatusCodes.Status401Unauthorized),
                       AccountConfirmationRequired => Results.Json(new SignInResponse(result.Status, result.Value), statusCode: StatusCodes.Status401Unauthorized),
                       Incorrect => Results.Unauthorized(),
                       TwoFactorIsNotEnabled => Results.Json(new SignInResponse(result.Status), statusCode: StatusCodes.Status403Forbidden),
                       TwoFactorRequired => Results.Json(new SignInResponse(result.Status, result.Value), statusCode: StatusCodes.Status403Forbidden),
                       _ => Results.Ok(new SignInResponse(result.Status, result.Value)),
                   };
        }
        catch (Exception ex) {
            logger.LogError(ex, "Sign in failed for user: {User}.", request.Identifier);
            return Results.Problem("An unexpected error occurred during the sign in.",
                                   statusCode: StatusCodes.Status500InternalServerError,
                                   title: "Sign-In Failed");
        }
    }

    internal static async Task<IResult> SignOutAsync(
        [FromServices] IAuthenticationService service,
        [FromBody] SignOutRequest request,
        [FromServices] ILoggerFactory loggerFactory) {
        var logger = loggerFactory.CreateLogger($"{nameof(UserEndpointMappings.MapUserAuthenticationEndpoints)}.{nameof(SignOutAsync)}");
        try {
            await service.SignOut(request);
            return Results.Ok();
        }
        catch (Exception ex) {
            logger.LogError(ex, "Sign out failed for user: {User}.", request.Identifier);
            return Results.Problem("An unexpected error occurred during the sign out.",
                                   statusCode: StatusCodes.Status500InternalServerError,
                                   title: "Sign Out Failed");
        }
    }

    internal static async Task<IResult> GetAuthenticationSchemesAsync(
        [FromServices] IAuthenticationService service,
        [FromServices] ILoggerFactory loggerFactory) {
        var logger = loggerFactory.CreateLogger($"{nameof(UserEndpointMappings.MapUserAuthenticationEndpoints)}.{nameof(GetAuthenticationSchemesAsync)}");
        try {
            var schemes = await service.GetSchemes();
            return Results.Ok(schemes);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Get authentication schemes failed.");
            return Results.Problem("An unexpected error occurred while getting the authentication schemes.",
                                   statusCode: StatusCodes.Status500InternalServerError,
                                   title: "Get Authentication GetAuthenticationSchemes Failed");
        }
    }
}
