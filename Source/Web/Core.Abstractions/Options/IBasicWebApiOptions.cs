namespace WebApi.Options;

public interface IBasicWebApiOptions
    : IWebApiOptions<IBasicWebApiOptions>;

public interface IBasicWebApiOptions<out TOptions>
    : IWebApiOptions<TOptions>
    where TOptions : IBasicWebApiOptions<TOptions>;
