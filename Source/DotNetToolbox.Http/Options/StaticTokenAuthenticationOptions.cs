﻿namespace DotNetToolbox.Http.Options;

public class StaticTokenAuthenticationOptions : AuthenticationOptions {
    public AuthenticationScheme Scheme { get; set; } = Basic;
    public string Token { get; set; } = string.Empty;

    internal override ValidationResult Validate() {
        var result = base.Validate();

        if (string.IsNullOrWhiteSpace(Token))
            result += new ValidationError(CannotBeNullOrWhiteSpace, nameof(Token));

        return result;
    }

    internal override void Configure(HttpClient client, ref HttpAuthentication authentication) {
        authentication = new() {
            Type = Jwt,
            Scheme = Scheme,
            Value = Token,
        };
        client.DefaultRequestHeaders.Authorization = authentication;
    }
}