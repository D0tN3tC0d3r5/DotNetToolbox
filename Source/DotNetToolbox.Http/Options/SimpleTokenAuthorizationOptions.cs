namespace DotNetToolbox.Http.Options;

public class SimpleTokenAuthorizationOptions : TokenAuthorizationOptions {
    internal override ValidationResult Validate(string? httpClientName = null) {
        var result = base.Validate(httpClientName);

        if (string.IsNullOrWhiteSpace(Token))
            result += new ValidationError(CannotBeNullOrWhiteSpace, GetSource(httpClientName, nameof(Token)));

        return result;
    }
}