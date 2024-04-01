namespace Sophia.Data;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment, string connectionStringName, Action<IdentityBuilder>? setIdentity = null) {
        var connectionString = configuration.GetConnectionString(connectionStringName)
                            ?? throw new InvalidOperationException("Connection string '{connectionStringName}' not found.");
        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging(environment.IsDevelopment());
            options.EnableDetailedErrors(environment.IsDevelopment());
            options.LogTo(Console.WriteLine, LogLevel.Information);
        });
        var identityBuilder = services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
        setIdentity?.Invoke(identityBuilder);

        services.AddScoped<DataContext, EntityFrameworkDataContext>();
        return services;
    }
}
