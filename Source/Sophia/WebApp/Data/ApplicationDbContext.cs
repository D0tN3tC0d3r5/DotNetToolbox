using Sophia.WebApp.Data.World;

namespace Sophia.WebApp.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options) {

    public required DbSet<WorldEntity> Worlds { get; set; }
    public required DbSet<SkillEntity> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        builder.Entity<WorldEntity>();
        builder.Entity<SkillEntity>();
    }
}
