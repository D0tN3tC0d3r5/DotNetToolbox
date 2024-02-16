namespace DotNetToolbox.Http.Options;

public class StaticTokenAuthenticationOptions : AuthenticationOptions {
    public AuthenticationScheme Scheme { get; set; } = Basic;
    public string Token { get; set; } = string.Empty;

    public override Result Validate(IDictionary<string, object?>? context = null) {
        var result = base.Validate(context);

        if (string.IsNullOrWhiteSpace(Token))
            result += new ValidationError(StringCannotBeNullOrWhiteSpace, GetSourcePath(nameof(Token)));

        return result;

        string GetSourcePath(string source)
            => context is null || !context.TryGetValue("ClientName", out var name)
                   ? source
                   : $"{name}.{source}";
    }

    internal override HttpAuthentication Configure(HttpClient client, HttpAuthentication _) {
        var authentication = new HttpAuthentication() {
            Type = Jwt,
            Scheme = Scheme,
            Value = Token,
        };
        client.DefaultRequestHeaders.Authorization = authentication;
        return authentication;
    }
}
