namespace WebApi.Builders;

public class BasicWebApiBuilder(string[] args)
    : BasicWebApiBuilder<BasicWebApiOptions>(args)
    , IBasicWebApiBuilder<WebApplication>;

public class BasicWebApiBuilder<TOptions>(string[] args)
    : WebApiBuilder<TOptions>(args)
    , IBasicWebApiBuilder<WebApplication, TOptions>
    where TOptions : BasicWebApiOptions<TOptions>, new();
