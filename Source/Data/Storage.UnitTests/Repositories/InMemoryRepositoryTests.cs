using DotNetToolbox.Results;

namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryRepositoryTests {
    private sealed class TestEntity()
        : IEntity<int> {
        public TestEntity(string name) : this() {
            Name = name;
        }
        public int Key { get; set; }
        public string Name { get; set; } = string.Empty;

        public Result Validate(IContext? context = null) => Result.Success();
    };

    private static readonly InMemoryRepository<TestEntity, int> _readOnlyRepo = [.. Enumerable.Range(0, 90).Select(x => new TestEntity($"{x}"))];
    private readonly InMemoryRepository<TestEntity, int> _updatableRepo = [new("A"), new("BB"), new("CCC")];

    private sealed class DummyRepository : Repository<DummyRepositoryStrategy, TestEntity, int>;
    private sealed class DummyRepositoryStrategy()
        : RepositoryStrategy<DummyRepositoryStrategy, DummyRepository, TestEntity, int>(new Lazy<DummyRepository>(() => new DummyRepository()));
    private static readonly DummyRepository _dummyRepository = [];

    [Fact]
    public void Constructor_Default_CreatesRepository() {
        var subject = new InMemoryRepository<TestEntity>();

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be(typeof(TestEntity));
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
        subject.AsyncProvider.Should().NotBeNull();
        subject.Id.Should().Match("|>Repository[TestEntity]_*<|");
    }
}
