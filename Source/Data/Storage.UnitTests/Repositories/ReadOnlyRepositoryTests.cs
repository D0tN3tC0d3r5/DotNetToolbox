namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyRepositoryTests {
    private readonly IRepositoryStrategy _strategy;
    private readonly ReadOnlyRepository<EntityWithKey, string> _keyedRepository;

    public ReadOnlyRepositoryTests() {
        _strategy = Substitute.For<IRepositoryStrategy>();
        _keyedRepository = new(_strategy);
    }

    private class EntityWithKey : IEntity<string> {
        public string Id { get; set; } = default!;
    }

    [Fact]
    public async Task Update_CallsExecuteAsyncOnStrategy() {
        var mockModel = new EntityWithKey();

        await _keyedRepository.CountAsync();

        await _strategy.Received().ExecuteAsync("Update", mockModel, default);
    }

    [Fact]
    public async Task AddOrUpdate_CallsExecuteAsyncOnStrategy() {
        var mockModel = new EntityWithKey();

        await _keyedRepository.HaveAny();

        await _strategy.Received().ExecuteAsync("AddOrUpdate", mockModel, default);
    }

    [Fact]
    public async Task Patch_CallsExecuteAsyncOnStrategy() {
        var key = "key";

        await _keyedRepository.FindByKey(key);

        await _strategy.Received().ExecuteAsync("Patch", key, default);
    }

    [Fact]
    public async Task CreateOrPatch_CallsExecuteAsyncOnStrategy() {
        const string key = "key";

        await _keyedRepository.FindFirst();

        await _strategy.Received().ExecuteAsync("CreateOrPatch", key, default);
    }

    [Fact]
    public async Task Remove_CallsExecuteAsyncOnStrategy() {
        const string key = "key";

        await _keyedRepository.GetListAsync();

        await _strategy.Received().ExecuteAsync("Remove", key, default);
    }
}
