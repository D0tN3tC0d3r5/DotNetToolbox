namespace Sophia.Data;

public class EntityFrameworkDataContext
    : DataContext {
    private readonly DbContext _dbContext;

    public EntityFrameworkDataContext(DbContext dbContext) {
        _dbContext = dbContext;
        Worlds = new WorldRepository(this, dbContext);
        Providers = new ProvidersRepository(this, dbContext);
        Models = new ModelsRepository(this, dbContext);
        Tools = new ToolsRepository(this, dbContext);
        Personas = new PersonasRepository(this, dbContext);
        Chats = new ChatsRepository(this, dbContext);
    }

    public override Repository<WorldData> Worlds { get; }
    public override Repository<ProviderData> Providers { get; }
    public override Repository<ModelData> Models { get; }
    public override Repository<ToolData> Tools { get; }
    public override Repository<PersonaData> Personas { get; }
    public override Repository<ChatData> Chats { get; }

    public override Task<int> SaveChanges(CancellationToken ct = default) => _dbContext.SaveChangesAsync(ct);
    public override async Task EnsureIsUpToDate(CancellationToken ct = default) {
        await _dbContext.Database.EnsureCreatedAsync(ct);
        await _dbContext.Database.MigrateAsync(ct);
        await base.EnsureIsUpToDate(ct);
    }
}
