namespace DotNetToolbox.Http.Options;

public abstract class AuthenticationOptions {
    internal virtual ValidationResult Validate()
        => Success();

    internal abstract void Configure(HttpClient client, ref HttpAuthentication authentication);
}