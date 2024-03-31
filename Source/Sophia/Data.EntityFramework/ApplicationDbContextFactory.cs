namespace Sophia.Data;

[ExcludeFromCodeCoverage]
public class ApplicationDbContextFactory
    : IDesignTimeDbContextFactory<ApplicationDbContext> {
    public ApplicationDbContext CreateDbContext(string[] args) {
        var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddUserSecrets<ApplicationDbContextFactory>();
        var config = builder.Build();

        var connectionString = config.GetConnectionString("Sophia.WebApp")
                            ?? throw new InvalidOperationException("Connection string 'Sophia.WebApp' not found.");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString, _ => _.MigrationsAssembly("Sophia.Data.EntityFramework"));
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        return new(optionsBuilder.Options);
    }
}
