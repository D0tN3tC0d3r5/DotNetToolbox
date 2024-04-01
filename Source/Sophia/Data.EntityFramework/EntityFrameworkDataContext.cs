namespace Sophia.Data;

public class EntityFrameworkDataContext
    : DataContext {
    private readonly ApplicationDbContext _dbContext;

    public EntityFrameworkDataContext(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
        Users = new UsersRepository(this, dbContext);
        Worlds = new WorldRepository(this, dbContext);
        Providers = new ProvidersRepository(this, dbContext);
        Models = new ModelsRepository(this, dbContext);
        Tools = new ToolsRepository(this, dbContext);
        Personas = new PersonasRepository(this, dbContext);
        Chats = new ChatsRepository(this, dbContext);
    }

    public override Repository<UserData, string> Users { get; }
    public override Repository<WorldData, Guid> Worlds { get; }
    public override Repository<ProviderData, int> Providers { get; }
    public override Repository<ModelData, string> Models { get; }
    public override Repository<ToolData, int> Tools { get; }
    public override Repository<PersonaData, int> Personas { get; }
    public override Repository<ChatData, Guid> Chats { get; }

    public override async Task<int> SaveChanges(CancellationToken ct = default)
        => await _dbContext.SaveChangesAsync(ct);
    public override async Task EnsureIsUpToDate(CancellationToken ct = default) {
        await _dbContext.Database.EnsureCreatedAsync(ct);
        await _dbContext.Database.MigrateAsync(ct);
        await base.EnsureIsUpToDate(ct);
        await _dbContext.SaveChangesAsync(ct);
    }
}
