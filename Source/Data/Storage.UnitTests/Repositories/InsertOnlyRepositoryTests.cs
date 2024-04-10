namespace DotNetToolbox.Data.Repositories;

public class InsertOnlyRepositoryTests {
    private readonly IRepositoryStrategy _strategy;
    private readonly InsertOnlyRepository<EntityWithKey, string> _keyedRepository;

    public InsertOnlyRepositoryTests() {
        _strategy = Substitute.For<IRepositoryStrategy>();
        _keyedRepository = new(_strategy);
    }

    private class EntityWithKey : IEntity<string> {
        public string Id { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
    }

    [Fact]
    public async Task Add_CallsExecuteAsyncOnStrategy() {
        var mockModel = new EntityWithKey();

        await _keyedRepository.Add(mockModel);

        await _strategy.Received().ExecuteAsync("Add", mockModel, default);
    }

    [Fact]
    public async Task Patch_CallsExecuteAsyncOnStrategy() {
        static void MockAction(EntityWithKey model) { }

        await _keyedRepository.Create(MockAction);

        await _strategy.Received().ExecuteAsync("Create", MockAction, default);
    }
}
