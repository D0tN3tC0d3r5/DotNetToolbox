namespace WebApi.Builders;

public interface IBasicWebApiBuilder<out THost>
    : IWebApiBuilder<THost, BasicWebApiOptions>
    where THost : IHost;

public interface IBasicWebApiBuilder<out THost, out TOptions>
    : IWebApiBuilder<THost, TOptions>
    where THost : IHost
    where TOptions : BasicWebApiOptions<TOptions>, new();
