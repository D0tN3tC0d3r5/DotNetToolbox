﻿namespace DotNetToolbox.Http.Options;

public class HttpClientOptions<TOptions> : NamedOptions<TOptions>, IValidatable
    where TOptions : HttpClientOptions<TOptions>, new() {
    public const string DefaultResponseFormat = "application/json";

    public virtual Uri? BaseAddress { get; set; }

    public virtual string ResponseFormat { get; set; } = DefaultResponseFormat;

    public virtual Dictionary<string, string[]> CustomHeaders { get; set; } = [];

    public virtual AuthenticationOptions? Authentication { get; set; }

    public virtual Result Validate(IDictionary<string, object?>? context = null) {
        var result = Success();

        if (BaseAddress is null)
            result += new ValidationError(ValueCannotBeNullOrWhiteSpace, GetSourcePath(nameof(BaseAddress)));

        result += Authentication?.Validate(context) ?? Success();

        return result;

        string GetSourcePath(string source)
            => context is null || !context.TryGetValue("ClientName", out var name)
                   ? source
                   : $"{name}.{source}";
    }
}

public class HttpClientOptions : HttpClientOptions<HttpClientOptions> {
    public Dictionary<string, HttpClientOptions> Clients { get; set; } = [];

    public override Result Validate(IDictionary<string, object?>? context = null) {
        var result = base.Validate(context);

        foreach (var client in Clients) {
            var clientContext = new Dictionary<string, object?> { ["ClientName"] = GetSourcePath(client.Key) };
            result += client.Value.Validate(clientContext);
        }

        return result;

        string GetSourcePath(string source)
            => context is null || !context.TryGetValue("ClientName", out var name)
                   ? source
                   : $"{name}.{source}";
    }
}
