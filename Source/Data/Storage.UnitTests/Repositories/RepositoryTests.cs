namespace DotNetToolbox.Data.Repositories;

public class RepositoryTests {
    private readonly IRepositoryStrategy _strategy;
    private readonly Repository<Entity> _repository;

    public RepositoryTests() {
        _strategy = Substitute.For<IRepositoryStrategy>();
        _repository = new(_strategy);
    }

    private class Entity : IEntity<string> {
        public string Id { get; set; } = default!;
    }

    [Fact]
    public async Task Update_CallsExecuteAsyncOnStrategy() {
        var mockModel = new Entity();

        await _repository.Update(mockModel);

        await _strategy.Received().ExecuteAsync("Update", mockModel, default);
    }

    [Fact]
    public async Task AddOrUpdate_CallsExecuteAsyncOnStrategy() {
        var mockModel = new Entity();

        await _repository.AddOrUpdate(mockModel);

        await _strategy.Received().ExecuteAsync("AddOrUpdate", mockModel, default);
    }

    [Fact]
    public async Task Patch_CallsExecuteAsyncOnStrategy() {
        const string key = "key";
        void Update(Entity _) { }

        await _repository.Patch(i => i.Id == key, Update);

        await _strategy.Received().ExecuteAsync<(Expression<Func<Entity, bool>>, Action<Entity>), Entity?>("Patch", Arg.Any<(Expression<Func<Entity, bool>>, Action<Entity>)>(), default);
    }

    [Fact]
    public async Task CreateOrPatch_CallsExecuteAsyncOnStrategy() {
        const string key = "key";
        static void Update(Entity _) { }

        await _repository.PatchOrCreate(i => i.Id == key, Update);

        await _strategy.Received().ExecuteAsync<(Expression<Func<Entity, bool>>, Action<Entity>), Entity?>("PatchOrCreate", Arg.Any<(Expression<Func<Entity, bool>>, Action<Entity>)>(), default);
    }

    [Fact]
    public async Task Remove_CallsExecuteAsyncOnStrategy() {
        var key = "key";

        await _repository.Remove(i => i.Id == key);

        await _strategy.Received().ExecuteAsync("Remove", Arg.Any<Expression<Func<Entity, bool>>>(), default);
    }
}
