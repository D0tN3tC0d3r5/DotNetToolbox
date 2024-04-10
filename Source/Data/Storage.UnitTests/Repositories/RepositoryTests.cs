namespace DotNetToolbox.Data.Repositories;

public class RepositoryTests {
    private readonly IRepositoryStrategy _strategy;
    private readonly Repository<EntityWithKey, string> _keyedRepository;

    public RepositoryTests() {
        _strategy = Substitute.For<IRepositoryStrategy>();
        _keyedRepository = new(_strategy);
    }

    private class EntityWithKey : IEntity<string> {
        public string Id { get; set; } = default!;
    }

    [Fact]
    public async Task Update_CallsExecuteAsyncOnStrategy() {
        var mockModel = new EntityWithKey();

        await _keyedRepository.Update(mockModel);

        await _strategy.Received().ExecuteAsync("Update", mockModel, default);
    }

    [Fact]
    public async Task AddOrUpdate_CallsExecuteAsyncOnStrategy() {
        var mockModel = new EntityWithKey();

        await _keyedRepository.AddOrUpdate(mockModel);

        await _strategy.Received().ExecuteAsync("AddOrUpdate", mockModel, default);
    }

    [Fact]
    public async Task Patch_CallsExecuteAsyncOnStrategy() {
        var model = new EntityWithKey();
        Task<EntityWithKey?> Find(CancellationToken _) => Task.FromResult<EntityWithKey?>(model);
        void Update(EntityWithKey _) { }

        await _keyedRepository.Patch("key", Update);

        await _strategy.Received().ExecuteAsync("Patch", ((Func<CancellationToken, Task<EntityWithKey?>>)Find, (Action<EntityWithKey>)Update), default);
    }

    [Fact]
    public async Task CreateOrPatch_CallsExecuteAsyncOnStrategy() {
        const string key = "key";
        static void MockAction(EntityWithKey _) { }

        await _keyedRepository.CreateOrPatch(key, MockAction);

        await _strategy.Received().ExecuteAsync("CreateOrPatch", (key, (Action<EntityWithKey>)MockAction), default);
    }

    [Fact]
    public async Task Remove_CallsExecuteAsyncOnStrategy() {
        var key = "key";

        await _keyedRepository.Remove(key);

        await _strategy.Received().ExecuteAsync("Remove", key, default);
    }
}
