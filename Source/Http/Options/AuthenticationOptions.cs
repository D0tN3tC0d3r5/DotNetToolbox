namespace DotNetToolbox.Http.Options;

public abstract class AuthenticationOptions : IValidatable {
    public virtual Result Validate(IDictionary<string, object?>? context = null)
        => Success();

    internal abstract HttpAuthentication Configure(HttpClient client, HttpAuthentication authentication);
}
