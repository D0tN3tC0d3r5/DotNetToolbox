// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting;

/// <summary>
/// Provides static factory methods for creating and configuring an Identity Provider Web API application.
/// Includes setup for ASP.NET Core Identity, user/role management services, data stores, and authentication.
/// </summary>
public static class IdentityProviderWebApi {
    /// <summary>
    /// Creates a new instance of a specific builder type <typeparamref name="TBuilder"/>
    /// and default options (<see cref="IdentityProviderWebApiOptions"/>).
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder, inheriting from <see cref="IdentityProviderWebApiBuilder{TUserDataStore}"/>.</typeparam>
    /// <typeparam name="TUserDataStore">The type of the user data store implementation.</typeparam>
    /// <param name="args">Command line arguments passed to the application.</param>
    /// <param name="setup">An optional action to further configure the builder after initial setup.</param>
    /// <returns>A configured <typeparamref name="TBuilder"/> instance.</returns>
    public static TBuilder CreateBuilder<TBuilder, TUserDataStore>(string[] args, Action<TBuilder>? setup = null)
        where TBuilder : IdentityProviderWebApiBuilder<TUserDataStore>
        where TUserDataStore : class, IUserStore<User>
        => CreateBuilder<TBuilder, TUserDataStore, User>(args, setup);

    /// <summary>
    /// Creates a new instance of a specific builder type <typeparamref name="TBuilder"/>
    /// and default options (<see cref="IdentityProviderWebApiOptions"/>).
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder, inheriting from <see cref="IdentityProviderWebApiBuilder{TUserDataStore, TUser}"/>.</typeparam>
    /// <typeparam name="TUserDataStore">The type of the user data store implementation.</typeparam>
    /// <typeparam name="TUser">The type of the user model.</typeparam>
    /// <param name="args">Command line arguments passed to the application.</param>
    /// <param name="setup">An optional action to further configure the builder after initial setup.</param>
    /// <returns>A configured <typeparamref name="TBuilder"/> instance.</returns>
    public static TBuilder CreateBuilder<TBuilder, TUserDataStore, TUser>(string[] args, Action<TBuilder>? setup = null)
        where TBuilder : IdentityProviderWebApiBuilder<TUserDataStore, TUser>
        where TUserDataStore : class, IUserStore<TUser>
        where TUser : User, new()
        => CreateBuilder<TBuilder, IdentityProviderWebApiOptions, TUserDataStore, TUser>(args, setup);

    /// <summary>
    /// Creates a new instance of a specific builder type <typeparamref name="TBuilder"/> with specific options <typeparamref name="TOptions"/>
    /// and specified data store, user, and role types. This is the core factory method.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the builder, inheriting from <see cref="IdentityProviderWebApiBuilder{TOptions, TUserDataStore, TUser}"/>.</typeparam>
    /// <typeparam name="TOptions">The type of the options record.</typeparam>
    /// <typeparam name="TUserDataStore">The type of the user data store implementation.</typeparam>
    /// <typeparam name="TUser">The type of the user model.</typeparam>
    /// <param name="args">Command line arguments passed to the application.</param>
    /// <param name="setup">An optional action to further configure the builder after initial setup.</param>
    /// <returns>A configured <typeparamref name="TBuilder"/> instance.</returns>
    public static TBuilder CreateBuilder<TBuilder, TOptions, TUserDataStore, TUser>(string[] args, Action<TBuilder>? setup = null)
        where TBuilder : IdentityProviderWebApiBuilder<TOptions, TUserDataStore, TUser>
        where TOptions : IdentityProviderWebApiOptions<TOptions>, new()
        where TUserDataStore : class, IUserStore<TUser>
        where TUser : User, new() {
        var builder = InstanceFactory.Create<TBuilder>((object)args);
        setup?.Invoke(builder);
        return builder;
    }

    /// <summary>
    /// Creates, configures, and builds the <see cref="WebApplication"/> using the default builder and options.
    /// </summary>
    /// <typeparam name="TBuilder">Specifies the type of builder used to create the web application.</typeparam>
    /// <typeparam name="TUserDataStore">The type of the user data store implementation.</typeparam>
    /// <param name="args">Command line arguments.</param>
    /// <param name="setup">Optional action to configure the builder.</param>
    /// <param name="configure">Optional action to configure the built application pipeline.</param>
    /// <returns>The configured <see cref="WebApplication"/>.</returns>
    public static WebApplication Build<TBuilder, TUserDataStore>(string[] args, Action<TBuilder>? setup = null, Action<WebApplication>? configure = null)
        where TBuilder : IdentityProviderWebApiBuilder<TUserDataStore>
        where TUserDataStore : class, IUserStore<User>
        => CreateBuilder<TBuilder, TUserDataStore, User>(args, setup).Build(configure);

    /// <summary>
    /// Creates, configures, and builds the <see cref="WebApplication"/> using the default builder and options.
    /// </summary>
    /// <typeparam name="TBuilder">Specifies the type of builder used to create the web application.</typeparam>
    /// <typeparam name="TUserDataStore">The type of the user data store implementation.</typeparam>
    /// <typeparam name="TUser">The type of the user model.</typeparam>
    /// <param name="args">Command line arguments.</param>
    /// <param name="setup">Optional action to configure the builder.</param>
    /// <param name="configure">Optional action to configure the built application pipeline.</param>
    /// <returns>The configured <see cref="WebApplication"/>.</returns>
    public static WebApplication Build<TBuilder, TUserDataStore, TUser>(string[] args, Action<TBuilder>? setup = null, Action<WebApplication>? configure = null)
        where TBuilder : IdentityProviderWebApiBuilder<TUserDataStore, TUser>
        where TUserDataStore : class, IUserStore<TUser>
        where TUser : User, new()
        => CreateBuilder<TBuilder, TUserDataStore, TUser>(args, setup).Build(configure);

    /// <summary>
    /// Creates and configures a web application with user authentication.
    /// </summary>
    /// <typeparam name="TBuilder">Specifies the type of builder used to create the web application.</typeparam>
    /// <typeparam name="TOptions">Defines the options that configure the behavior of the multi-user web application.</typeparam>
    /// <typeparam name="TUserDataStore">Represents the data store that manages user-specific data.</typeparam>
    /// <typeparam name="TUser">The type of the user model.</typeparam>
    /// <param name="args">Contains command-line arguments for configuring the application at startup.</param>
    /// <param name="setup">Allows for additional setup actions to be performed on the builder before the application is built.</param>
    /// <param name="configure">Enables further configuration of the web application after it has been built.</param>
    /// <returns>Returns the configured web application instance ready for use.</returns>
    public static WebApplication Build<TBuilder, TOptions, TUserDataStore, TUser>(string[] args, Action<TBuilder>? setup = null, Action<WebApplication>? configure = null)
        where TBuilder : IdentityProviderWebApiBuilder<TOptions, TUserDataStore, TUser>
        where TOptions : IdentityProviderWebApiOptions<TOptions>, new()
        where TUserDataStore : class, IUserStore<TUser>
        where TUser : User, new()
        => CreateBuilder<TBuilder, TOptions, TUserDataStore, TUser>(args, setup).Build(configure);

    /// <summary>
    /// Creates, configures, builds, and runs the <see cref="WebApplication"/> using the default builder and options.
    /// </summary>
    /// <typeparam name="TBuilder">Defines the type responsible for building the multi-user web API.</typeparam>
    /// <typeparam name="TUserDataStore">The type of the user data store implementation.</typeparam>
    /// <param name="args">Command line arguments.</param>
    /// <param name="setup">Optional action to configure the builder.</param>
    /// <param name="configure">Optional action to configure the built application pipeline.</param>
    public static void Run<TBuilder, TUserDataStore>(string[] args, Action<TBuilder>? setup = null, Action<WebApplication>? configure = null)
        where TBuilder : IdentityProviderWebApiBuilder<TUserDataStore>
        where TUserDataStore : class, IUserStore<User>
        => Build<TBuilder, TUserDataStore, User>(args, setup, configure).Run();

    /// <summary>
    /// Creates, configures, builds, and runs the <see cref="WebApplication"/> using the default builder and options.
    /// </summary>
    /// <typeparam name="TBuilder">Defines the type responsible for building the multi-user web API.</typeparam>
    /// <typeparam name="TUserDataStore">The type of the user data store implementation.</typeparam>
    /// <typeparam name="TUser">The type of the user model.</typeparam>
    /// <param name="args">Command line arguments.</param>
    /// <param name="setup">Optional action to configure the builder.</param>
    /// <param name="configure">Optional action to configure the built application pipeline.</param>
    public static void Run<TBuilder, TUserDataStore, TUser>(string[] args, Action<TBuilder>? setup = null, Action<WebApplication>? configure = null)
        where TBuilder : IdentityProviderWebApiBuilder<TUserDataStore, TUser>
        where TUserDataStore : class, IUserStore<TUser>
        where TUser : User, new()
        => Build<TBuilder, TUserDataStore, TUser>(args, setup, configure).Run();

    /// <summary>
    /// Executes the application with specified user configurations and optional setup or configuration actions.
    /// </summary>
    /// <typeparam name="TBuilder">Defines the type responsible for building the multi-user web API.</typeparam>
    /// <typeparam name="TOptions">Specifies the options that configure the multi-user behavior of the web API.</typeparam>
    /// <typeparam name="TUserDataStore">Represents the data store used to manage user-specific data.</typeparam>
    /// <typeparam name="TUser">The type of the user model.</typeparam>
    /// <param name="args">Contains the command-line arguments passed to the application.</param>
    /// <param name="setup">Allows for custom setup actions to be performed on the builder before running the application.</param>
    /// <param name="configure">Enables additional configuration of the web application before it starts.</param>
    public static void Run<TBuilder, TOptions, TUserDataStore, TUser>(string[] args, Action<TBuilder>? setup = null, Action<WebApplication>? configure = null)
        where TBuilder : IdentityProviderWebApiBuilder<TOptions, TUserDataStore, TUser>
        where TOptions : IdentityProviderWebApiOptions<TOptions>, new()
        where TUserDataStore : class, IUserStore<TUser>
        where TUser : User, new()
        => Build<TBuilder, TOptions, TUserDataStore, TUser>(args, setup, configure).Run();
}
