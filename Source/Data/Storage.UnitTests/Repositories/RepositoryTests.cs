using DotNetToolbox.Domain.Models;

namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    private sealed class TestEntity()
        : IEntity<int> {
        public TestEntity(string name) : this() {
            Name = name;
        }
        public int Key { get; set; }
        public string Name { get; set; } = string.Empty;
    };

    private static readonly Repository<TestEntity, int> _repo = new([new("A"), new("BB"), new("CCC")]);
    private static readonly ChunkedRepository<TestEntity, int> _chunkedRepo = new(Enumerable.Range(0, 90).ToArray(x => new TestEntity($"{x}")));
    private static readonly PagedRepository<TestEntity, int> _pagedRepo= new(Enumerable.Range(0, 90).ToArray(x => new TestEntity($"{x}")));
    private readonly Repository<TestEntity, int> _updatableRepo = [new("A"), new("BB"), new("CCC")];

    private sealed class DummyRepositoryStrategy : RepositoryStrategy<DummyRepositoryStrategy, TestEntity>;
    private sealed class DummyRepository : RepositoryBase<DummyRepositoryStrategy, TestEntity>;
    private static readonly DummyRepository _dummyRepository = [];
    private sealed class DummyChunkedRepository : ChunkedRepositoryBase<DummyRepositoryStrategy, TestEntity>;
    private static readonly DummyChunkedRepository _dummyChunkedRepository = [];
    private sealed class DummyPagedRepository : PagedRepositoryBase<DummyRepositoryStrategy, TestEntity>;
    private static readonly DummyPagedRepository _dummyPagedRepository = [];

    [Fact]
    public void Constructor_Default_CreatesRepository() {
        var subject = new Repository<TestEntity>();

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be(typeof(TestEntity));
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
        subject.AsyncProvider.Should().NotBeNull();
        subject.Name.Should().Match("|>Repository[TestEntity]_*<|");
    }

    [Fact]
    public void Constructor_WithSeed_CreatesRepository() {
        var subject = new Repository<TestEntity>([new("A"), new("BB"), new("CCC")]);
        subject.Should().NotBeNull();
        subject.Count().Should().Be(3);
        subject.Name.Should().Match("|>Repository[TestEntity]_*<|");
    }

    [Fact]
    public void Constructor_WithName_CreatesRepository() {
        var subject = new Repository<TestEntity>("MyRepository");

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be(typeof(TestEntity));
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
        subject.AsyncProvider.Should().NotBeNull();
        subject.Name.Should().Match("MyRepository");
    }

    [Fact]
    public void Constructor_WithNameAndSeed_CreatesRepository() {
        var subject = new Repository<TestEntity>("MyRepository", [new("A"), new("BB"), new("CCC")]);
        subject.Should().NotBeNull();
        subject.Count().Should().Be(3);
        subject.Name.Should().Match("MyRepository");
    }
}
