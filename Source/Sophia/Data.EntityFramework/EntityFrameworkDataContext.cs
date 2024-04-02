namespace Sophia.Data;

public class EntityFrameworkDataContext(ApplicationDbContext dbContext)
    : DataContext {
    public override UserRepository Users { get; } = new UsersDbSet(dbContext);
    public override WorldRepository Worlds { get; } = new WorldDbSet(dbContext);
    public override ProviderRepository Providers { get; } = new ProvidersDbSet(dbContext);
    public override ModelRepository Models { get; } = new ModelsDbSet(dbContext);
    public override ToolRepository Tools { get; } = new ToolsDbSet(dbContext);
    public override PersonaRepository Personas { get; } = new PersonasDbSet(dbContext);
    public override ChatRepository Chats { get; } = new ChatsDbSet(dbContext);

    public override Task<int> SaveChanges(CancellationToken ct = default)
        => dbContext.SaveChangesAsync(ct);
    public override async Task EnsureIsUpToDate(CancellationToken ct = default) {
        await dbContext.Database.EnsureCreatedAsync(ct);
        await dbContext.Database.MigrateAsync(ct);
        await base.EnsureIsUpToDate(ct);
        await dbContext.SaveChangesAsync(ct);
    }
}
