namespace WebApi.Builders;

public interface IWebApiBuilder<out THost, out TOptions>
    : IHostApplicationBuilder
    where THost : IHost
    where TOptions : WebApiOptions<TOptions>, new() {
    TOptions Options { get; }

    THost Build(Action<THost>? configure = null);
}
