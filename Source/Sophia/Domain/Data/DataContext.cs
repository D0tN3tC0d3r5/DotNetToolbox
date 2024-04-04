namespace Sophia.Data;

public abstract class DataContext {

    public abstract UserRepository Users { get; }
    public abstract WorldRepository Worlds { get; }
    public abstract ProviderRepository Providers { get; }
    public abstract ModelRepository Models { get; }
    public abstract ToolRepository Tools { get; }
    public abstract PersonaRepository Personas { get; }
    public abstract ChatRepository Chats { get; }

    public abstract Task<int> SaveChanges(CancellationToken ct = default);
    public virtual Task EnsureIsUpToDate(CancellationToken ct = default) => Seed(ct);

    protected virtual async Task Seed(CancellationToken ct = default) {
        await SeedWorld(ct);
        await SeedProviders(ct);
        await SeedPersonas(ct);
        await SaveChanges(ct);
    }

    private async Task SeedWorld(CancellationToken ct = default) {
        if (await Worlds.HaveAny(ct))
            return;
        var world = new WorldData();
        await Worlds.Add(world, ct);
    }

    private async Task SeedProviders(CancellationToken ct = default) {
        await TryAddOpenAI(ct);
        await TryAddAnthropic(ct);
    }

    private async Task SeedPersonas(CancellationToken ct = default) {
        if (await Personas.HaveAny(i => i.Name == "Assistant", ct))
            return;
        var persona = new PersonaData {
            Name = "Assistant",
            Description = "You are a helpful assistant.",
        };
        await Personas.Add(persona, ct);
    }

    private async Task TryAddOpenAI(CancellationToken ct = default) {
        if (await Providers.HaveAny(i => i.Name == "OpenAI", ct))
            return;
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
        if (await Providers.HaveAny(i => i.Name == "Anthropic", ct))
            return;
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
