namespace Sophia.WebApp.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options) {

    public required DbSet<WorldEntity> Worlds { get; set; }
    public required DbSet<ToolEntity> Tools { get; set; }
    public required DbSet<PersonaEntity> Personas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<WorldEntity>();
        builder.Entity<ToolEntity>();
        builder.Entity<PersonaEntity>();
    }

    public static async Task Seed(IServiceProvider services) {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
        await WorldEntity.Seed(dbContext);
        await dbContext.SaveChangesAsync();
    }
}
