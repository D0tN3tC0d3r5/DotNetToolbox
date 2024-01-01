namespace DotNetToolbox.ConsoleApplication;

public class ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    : IApplicationBuilder<TApplication, TBuilder, TOptions>
    where TApplication : ShellApplication<TApplication, TBuilder, TOptions>
    where TBuilder : ShellApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : ApplicationOptions<TOptions>, new() {
    private readonly TOptions _options;

    internal ShellApplicationBuilder(string[] args) {
        _options = new();
        var configuration = new ConfigurationManager();

        configuration.AddCommandLine(args);
        configuration.AddEnvironmentVariables();
        configuration.AddJsonFile("appsettings.json");

        //_hostApplicationBuilder = new HostApplicationBuilder(new HostApplicationBuilderSettings {
        //                                                                                            Args = options.Args,
        //                                                                                            ApplicationName = options.ApplicationName,
        //                                                                                            EnvironmentName = options.EnvironmentName,
        //                                                                                            ContentRootPath = options.ContentRootPath,
        //                                                                                            Configuration = configuration,
        //                                                                                        });

        //// Set WebRootPath if necessary
        //if (options.WebRootPath is not null) {
        //    Configuration.AddInMemoryCollection(new[]
        //                                        {
        //                                            new KeyValuePair<string, string?>(WebHostDefaults.WebRootKey, options.WebRootPath),
        //                                        });
        //}

        //// Run methods to configure web host defaults early to populate services
        //var bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);

        //// This is for testing purposes
        //configureDefaults?.Invoke(bootstrapHostBuilder);

        //bootstrapHostBuilder.ConfigureWebHostDefaults(webHostBuilder => {
        //                                                  // Runs inline.
        //                                                  webHostBuilder.Configure(ConfigureApplication);

        //                                                  InitializeWebHostSettings(webHostBuilder);
        //                                              },
        //                                              options => {
        //                                                  // We've already applied "ASPNETCORE_" environment variables to hosting config
        //                                                  options.SuppressEnvironmentConfiguration = true;
        //                                              });

        //_genericWebHostServiceDescriptor = InitializeHosting(bootstrapHostBuilder);
    }

    public TApplication Build() => throw new NotImplementedException();
}

public class ShellApplicationBuilder
    : ShellApplicationBuilder<ShellApplication, ShellApplicationBuilder, ApplicationOptions> {
    internal ShellApplicationBuilder(string[] args)
        : base(args) {
    }
}
