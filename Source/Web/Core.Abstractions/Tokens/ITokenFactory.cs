namespace WebApi.Tokens;

/// <summary>
/// Defines methods for creating tokens. Supports customizable parameters like type, value, and lifetime.
/// </summary>
public interface ITokenFactory {
    /// <summary>
    /// Generates a temporary token based on specified type and value.
    /// </summary>
    /// <param name="options">Specifies the configuration settings for the temporary token generation.</param>
    /// <param name="type">Specifies the category or purpose of the token being created.</param>
    /// <param name="value">Holds the specific data or identifier associated with the token.</param>
    /// <param name="delayStartBySeconds">Indicates the duration to wait before the token becomes active.</param>
    /// <returns>Provides a temporary token object that encapsulates the generated token information.</returns>
    TemporaryToken CreateTemporaryToken(TemporaryTokenOptions options, string type, string value, uint delayStartBySeconds = 0);
    /// <summary>
    /// Generates an access token based on specified options and a claims identity. The token can be delayed before
    /// activation.
    /// </summary>
    /// <param name="options">Specifies the configuration settings for the access token generation.</param>
    /// <param name="subject">Represents the identity claims associated with the access token.</param>
    /// <param name="delayStartBySeconds">Indicates the number of seconds to wait before the token becomes active.</param>
    /// <returns>Returns an access token that can be used for authentication.</returns>
    AccessToken CreateAccessToken(AccessTokenOptions options, ClaimsIdentity subject, uint delayStartBySeconds = 0);
}
