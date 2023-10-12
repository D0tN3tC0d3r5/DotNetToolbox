namespace DotNetToolbox.Http.Options;

public abstract record AuthenticationOptions {
    internal virtual ValidationResult Validate(string? httpClientName = null)
        => Success();

    protected static string GetSource(string? name, params string[] fields)
        => $"{(name is null ? string.Empty : $"{name}.")}"
         + $"{string.Join(".", fields)}";

    internal abstract void Configure(HttpClient client, ref HttpClientAuthentication authentication);
}