namespace DotNetToolbox.Http.Options;

public abstract class AuthenticationOptions {
    internal virtual Result Validate()
        => Success();

    internal abstract void Configure(HttpClient client, ref HttpAuthentication authentication);
}