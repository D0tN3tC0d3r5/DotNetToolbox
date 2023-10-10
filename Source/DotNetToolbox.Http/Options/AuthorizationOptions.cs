using static DotNetToolbox.Http.Options.AuthorizationType;

namespace DotNetToolbox.Http.Options;

public class AuthorizationOptions {

    public AuthorizationType Type { get; set; }

    internal virtual ValidationResult Validate(string? httpClientName = null)
        => Success();

    protected static string GetSource(string? name, params string[] fields)
        => $"{(name is null ? string.Empty : $"{name}.")}"
         + $"{string.Join(".", fields)}";
}