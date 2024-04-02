namespace Sophia.Data;

public class EntityFrameworkDataContext(ApplicationDbContext dbContext)
    : DataContext {
    public override Repository<UserData, string> Users { get; }
        = new UsersRepository(dbContext);
    public override Repository<WorldData, Guid> Worlds { get; }
        = new WorldRepository(dbContext);
    public override Repository<ProviderData, int> Providers { get; }
        = new ProvidersRepository(dbContext);
    public override Repository<ModelData, string> Models { get; }
        = new ModelsRepository(dbContext);
    public override Repository<ToolData, int> Tools { get; }
        = new ToolsRepository(dbContext);
    public override Repository<PersonaData, int> Personas { get; }
        = new PersonasRepository(dbContext);
    public override Repository<ChatData, Guid> Chats { get; }
        = new ChatsRepository(dbContext);

    public override Task<int> SaveChanges(CancellationToken ct = default)
        => dbContext.SaveChangesAsync(ct);
    public override async Task EnsureIsUpToDate(CancellationToken ct = default) {
        await dbContext.Database.EnsureCreatedAsync(ct);
        await dbContext.Database.MigrateAsync(ct);
        await base.EnsureIsUpToDate(ct);
        await dbContext.SaveChangesAsync(ct);
    }
}
