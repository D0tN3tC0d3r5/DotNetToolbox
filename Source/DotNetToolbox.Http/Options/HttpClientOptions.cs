﻿namespace DotNetToolbox.Http.Options;

public class HttpClientOptions  {
    public const string DefaultResponseFormat = "application/json";

    public string BaseAddress { get; set; } = default!;

    public string ResponseFormat { get; set; } = DefaultResponseFormat;

    public Dictionary<string, string[]> CustomHeaders { get; set; } = new();

    public AuthenticationOptions? Authentication { get; set; }

    public ValidationResult Validate() {
        var result = Success();

        if (string.IsNullOrWhiteSpace(BaseAddress))
            result += new ValidationError(CannotBeNullOrWhiteSpace, nameof(BaseAddress));

        if (string.IsNullOrWhiteSpace(ResponseFormat))
            result += new ValidationError(CannotBeNullOrWhiteSpace, nameof(ResponseFormat));

        result += Authentication?.Validate() ?? Success();

        return result;
    }
    internal void Configure(HttpClient client, ref HttpAuthentication authentication) {
        client.BaseAddress = new(BaseAddress);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new(ResponseFormat));
        foreach ((var key, var value) in CustomHeaders)
            client.DefaultRequestHeaders.Add(key, value);

        Authentication?.Configure(client, ref authentication);
    }
}