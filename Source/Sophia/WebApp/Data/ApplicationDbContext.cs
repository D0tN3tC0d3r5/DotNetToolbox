using Sophia.WebApp.Data.Tools;

namespace Sophia.WebApp.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options) {

    public required DbSet<WorldEntity> Worlds { get; set; }
    public required DbSet<ToolEntity> Tools { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<WorldEntity>();
        builder.Entity<ToolEntity>();
    }
}
