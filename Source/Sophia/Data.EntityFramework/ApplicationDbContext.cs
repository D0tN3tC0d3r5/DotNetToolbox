namespace Sophia.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<User>(options) {
    public DbSet<WorldEntity> Worlds { get; set; } = default!;
    public DbSet<ProviderEntity> Providers { get; set; } = default!;
    public DbSet<ModelEntity> Models { get; set; } = default!;
    public DbSet<ToolEntity> Tools { get; set; } = default!;
    public DbSet<PersonaEntity> Personas { get; set; } = default!;
    public DbSet<ChatEntity> Chats { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<User>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

        builder.Entity<WorldEntity>();
        builder.Entity<ProviderEntity>();
        builder.Entity<ModelEntity>();
        builder.Entity<ToolEntity>();
        builder.Entity<PersonaEntity>();
        builder.Entity<ChatEntity>();
    }
}
