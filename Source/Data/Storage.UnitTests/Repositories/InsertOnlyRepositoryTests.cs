namespace DotNetToolbox.Data.Repositories;

public class InsertOnlyRepositoryTests {
    private readonly IRepositoryStrategy _strategy;
    private readonly InsertOnlyRepository<Entity> _repository;
    private readonly InsertOnlyRepository<int> _set = new([1, 2, 3]);

    public InsertOnlyRepositoryTests() {
        _strategy = Substitute.For<IRepositoryStrategy>();
        _repository = new(_strategy);
    }

    private class Entity : IEntity<string> {
        public string Id { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
    }

    [Fact]
    public async Task Add_CallsExecuteAsyncOnStrategy() {
        var mockModel = new Entity();

        await _repository.Add(mockModel);

        await _strategy.Received().ExecuteAsync("Add", mockModel, default);
    }

    [Fact]
    public async Task Create_CallsExecuteAsyncOnStrategy() {
        static void Set(Entity model) { }

        await _repository.Create(Set);

        await _strategy.Received().ExecuteAsync<Action<Entity>, Entity>("Create", Set, default);
    }

    [Fact]
    public async Task Add_ForInMemory_ReturnsCount() {
        await _set.Add(4);

        _set.Count().Should().Be(4);
    }

    [Fact]
    public async Task Create_ForInMemory_Returns() {
        var result = await _set.Create(i => {});

        result.Should().Be(4);
        _set.Count().Should().Be(4);
    }
}
