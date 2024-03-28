namespace Sophia.WebApp.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options) {

    public required DbSet<WorldEntity> Worlds { get; set; }
    public required DbSet<ProviderEntity> Providers { get; set; }
    public required DbSet<ToolEntity> Tools { get; set; }
    public required DbSet<PersonaEntity> Personas { get; set; }
    public required DbSet<ChatEntity> Chats { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        builder.Entity<WorldEntity>();
        builder.Entity<ProviderEntity>();
        builder.Entity<ToolEntity>();
        builder.Entity<PersonaEntity>();
        builder.Entity<ChatEntity>();
    }

    public static async Task Seed(IServiceProvider services) {
        await using var scope = services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Database.MigrateAsync();
        await WorldEntity.Seed(dbContext);
        await ProviderEntity.Seed(dbContext);
        await PersonaEntity.Seed(dbContext);
        await dbContext.SaveChangesAsync();
    }
}
