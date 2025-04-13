// ReSharper disable once CheckNamespace
namespace WebApi;

/// <summary>
/// Provides static factory methods for creating and configuring a basic Web API application.
/// Includes essential services like caching, problem details, OpenAPI support, token factories, and Aspire service defaults.
/// </summary>
public static class BasicWebApi {
    /// <summary>
    /// Creates a new instance of the default <see cref="BasicWebApiBuilder"/>.
    /// </summary>
    /// <param name="args">Command line arguments passed to the application.</param>
    /// <param name="setup">An optional action to further configure the builder after initial setup.</param>
    /// <returns>A configured <see cref="BasicWebApiBuilder"/> instance.</returns>
    public static BasicWebApiBuilder CreateBuilder(string[] args, Action<BasicWebApiBuilder>? setup = null)
        => CreateBuilder<BasicWebApiBuilder>(args, setup);

    /// <summary>
    /// Creates a new instance of a specific builder type <typeparamref name="TBuilder"/>.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder, inheriting from <see cref="BasicWebApiBuilder"/>.</typeparam>
    /// <param name="args">Command line arguments passed to the application.</param>
    /// <param name="setup">An optional action to further configure the builder.</param>
    /// <returns>A configured <typeparamref name="TBuilder"/> instance.</returns>
    public static TBuilder CreateBuilder<TBuilder>(string[] args, Action<TBuilder>? setup = null)
        where TBuilder : BasicWebApiBuilder
        => CreateBuilder<TBuilder, BasicWebApiOptions>(args, setup);

    /// <summary>
    /// Creates a new instance of a specific builder type <typeparamref name="TBuilder"/> with specific options <typeparamref name="TOptions"/>.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder, inheriting from <see cref="BasicWebApiBuilder{TOptions}"/>.</typeparam>
    /// <typeparam name="TOptions">The type of the options record.</typeparam>
    /// <param name="args">Command line arguments passed to the application.</param>
    /// <param name="setup">An optional action to further configure the builder.</param>
    /// <returns>A configured <typeparamref name="TBuilder"/> instance.</returns>
    public static TBuilder CreateBuilder<TBuilder, TOptions>(string[] args, Action<TBuilder>? setup = null)
        where TBuilder : BasicWebApiBuilder<TOptions>
        where TOptions : BasicWebApiOptions<TOptions>, new() {
        var builder = InstanceFactory.Create<TBuilder>((object)args);
        setup?.Invoke(builder);
        return builder;
    }

    /// <summary>
    /// Creates, configures, and builds the <see cref="WebApplication"/> using the default builder and options.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <param name="setup">Optional action to configure the builder.</param>
    /// <param name="configure">Optional action to configure the built application pipeline.</param>
    /// <returns>The configured <see cref="WebApplication"/>.</returns>
    public static WebApplication Build(string[] args, Action<BasicWebApiBuilder>? setup = null, Action<WebApplication>? configure = null)
        => CreateBuilder(args, setup).Build(configure);

    /// <summary>
    /// Creates and configures a web application using specified builder and options types.
    /// </summary>
    /// <typeparam name="TBuilder">Specifies the type of builder used to create the web application.</typeparam>
    /// <param name="args">Contains command-line arguments for configuring the web application.</param>
    /// <param name="setup">An optional action to perform additional setup on the builder.</param>
    /// <param name="configure">An optional action to configure the web application after it has been built.</param>
    /// <returns>Returns the configured web application instance.</returns>
    public static WebApplication Build<TBuilder>(string[] args, Action<TBuilder>? setup = null, Action<WebApplication>? configure = null)
        where TBuilder : BasicWebApiBuilder
        => CreateBuilder(args, setup).Build(configure);

    /// <summary>
    /// Creates and configures a web application using specified builder and options types.
    /// </summary>
    /// <typeparam name="TBuilder">Specifies the type of builder used to create the web application.</typeparam>
    /// <typeparam name="TOptions">Defines the options type that configures the web application settings.</typeparam>
    /// <param name="args">Contains command-line arguments for configuring the web application.</param>
    /// <param name="setup">An optional action to perform additional setup on the builder.</param>
    /// <param name="configure">An optional action to configure the web application after it has been built.</param>
    /// <returns>Returns the configured web application instance.</returns>
    public static WebApplication Build<TBuilder, TOptions>(string[] args, Action<TBuilder>? setup = null, Action<WebApplication>? configure = null)
        where TBuilder : BasicWebApiBuilder<TOptions>
        where TOptions : BasicWebApiOptions<TOptions>, new()
        => CreateBuilder<TBuilder, TOptions>(args, setup).Build(configure);

    /// <summary>
    /// Creates, configures, builds, and runs the <see cref="WebApplication"/> using the default builder and options.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <param name="setup">Optional action to configure the builder.</param>
    /// <param name="configure">Optional action to configure the built application pipeline.</param>
    public static void Run(string[] args, Action<BasicWebApiBuilder>? setup = null, Action<WebApplication>? configure = null)
        => Build(args, setup, configure).Run();

    /// <summary>
    /// Creates, configures, builds, and runs the <see cref="WebApplication"/> using a specific options type.
    /// </summary>
    /// <typeparam name="TBuilder">Specifies the type of builder used to create the web application.</typeparam>
    /// <param name="args">Command line arguments.</param>
    /// <param name="setup">Optional action to configure the builder.</param>
    /// <param name="configure">Optional action to configure the built application pipeline.</param>
    public static void Run<TBuilder>(string[] args, Action<TBuilder>? setup = null, Action<WebApplication>? configure = null)
        where TBuilder : BasicWebApiBuilder
        => Build(args, setup, configure).Run();

    /// <summary>
    /// Creates, configures, builds, and runs the <see cref="WebApplication"/> using a specific options type.
    /// </summary>
    /// <typeparam name="TBuilder">Specifies the type of builder used to create the web application.</typeparam>
    /// <typeparam name="TOptions">Defines the options type that configures the web application settings.</typeparam>
    /// <param name="args">Command line arguments.</param>
    /// <param name="setup">Optional action to configure the builder.</param>
    /// <param name="configure">Optional action to configure the built application pipeline.</param>
    public static void Run<TBuilder, TOptions>(string[] args, Action<TBuilder>? setup = null, Action<WebApplication>? configure = null)
        where TBuilder : BasicWebApiBuilder<TOptions>
        where TOptions : BasicWebApiOptions<TOptions>, new()
        => Build<TBuilder, TOptions>(args, setup, configure).Run();
}
