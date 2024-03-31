namespace Sophia.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options) {

    public DbSet<WorldEntity> Worlds { get; set; }
    public DbSet<ProviderEntity> Providers { get; set; }
    public DbSet<ModelEntity> Models { get; set; }
    public DbSet<ToolEntity> Tools { get; set; }
    public DbSet<PersonaEntity> Personas { get; set; }
    public DbSet<ChatEntity> Chats { get; set; }

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
}
