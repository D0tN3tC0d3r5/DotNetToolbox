using SignInResult = WebApi.Model.SignInResult;

namespace WebApi.Contracts.Authentication;

/// <summary>
/// Represents the response for a sign-in operation, including a token and its expiration time.
/// Indicates if two-factor authentication or confirmation is required.
/// </summary>
public sealed record SignInResponse()
    : Response {
    public SignInResponse(SignInResult result, TemporaryToken? token = null)
        : this() {
        Result = result;
        Token = token;
    }

    /// <summary>
    /// Represents the outcome of a sign-in attempt. It indicates whether the sign-in was successful or failed.
    /// </summary>
    public SignInResult Result { get; init; }
    /// <summary>
    /// Represents the associated with the signin.
    /// It only has a valid value if the sign in is successful.
    /// </summary>
    public TemporaryToken? Token { get; init; }
}
