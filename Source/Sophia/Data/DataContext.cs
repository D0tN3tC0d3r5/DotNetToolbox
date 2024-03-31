namespace Sophia.Data;

public abstract class DataContext {

    public abstract Repository<WorldData> Worlds { get; }
    public abstract Repository<ProviderData> Providers { get; }
    public abstract Repository<ModelData> Models { get; }
    public abstract Repository<ToolData> Tools { get; }
    public abstract Repository<PersonaData> Personas { get; }
    public abstract Repository<ChatData> Chats { get; }

    public abstract Task<int> SaveChanges(CancellationToken ct = default);
    public virtual Task EnsureIsUpToDate(CancellationToken ct = default) => Seed(ct);

    protected virtual async Task Seed(CancellationToken ct = default) {
        await SeedWorld(ct);
        await SeedProviders(ct);
        await SeedPersonas(ct);
        await SaveChanges(ct);
    }

    private async Task SeedWorld(CancellationToken ct = default) {
        if (Worlds.Any()) return;
        var world = new WorldData();
        await Worlds.Add(world, ct);
    }

    private async Task SeedProviders(CancellationToken ct = default) {
        await TryAddOpenAI(ct);
        await TryAddAnthropic(ct);
    }

    private async Task SeedPersonas(CancellationToken ct = default) {
        if (Personas.Any()) return;
        var persona = new PersonaData();
        await Personas.Add(persona, ct);
    }

    private async Task TryAddOpenAI(CancellationToken ct = default) {
        if (Providers.Any(i => i.Name == "OpenAI")) return;
        var provider = new ProviderData {
            Name = "OpenAI",
            Models = [
                         new() {
                                   Id = "gpt-4-turbo-preview",
                                   Name = "GPT 4 Turbo",
                               },
                         new() {
                                   Id = "gpt-3.5-turbo",
                                   Name = "GPT 3.5 Turbo",
                               },
                     ],
        };
        await Providers.Add(provider, ct);
    }

    private async Task TryAddAnthropic(CancellationToken ct = default) {
        if (Providers.Any(i => i.Name == "Anthropic")) return;
        var provider = new ProviderData {
            Name = "Anthropic",
            Models = [
                         new() {
                                   Id = "claude-3-opus-20240229",
                                   Name = "Claude 3 Opus",
                               },
                         new() {
                                   Id = "claude-3-haiku-20240307",
                                   Name = "Claude 3 Haiku",
                               },
                         new() {
                                   Id = "claude-2.1",
                                   Name = "Claude 2.1",
                               },
                     ],
        };
        await Providers.Add(provider, ct);
    }
}
