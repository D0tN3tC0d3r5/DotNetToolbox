namespace WebApi.Options;

public record BasicWebApiOptions
    : BasicWebApiOptions<BasicWebApiOptions>
    , IBasicWebApiOptions;

public record BasicWebApiOptions<TOptions>
    : WebApiOptions<TOptions>
    , IBasicWebApiOptions<TOptions>
    where TOptions : BasicWebApiOptions<TOptions>, new();
