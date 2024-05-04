using DotNetToolbox.Data.Strategies;

namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    private abstract record BaseTestEntity(string Name);
    private sealed record TestEntity(string Name) : BaseTestEntity(Name);

    private static readonly Repository<BaseTestEntity> _emptyRepo = [];
    private static readonly Repository<TestEntity> _singleElementRepo = [new("A")];
    private static readonly Repository<TestEntity> _repo = new([new("A"), new("BB"), new("CCC")]);
    private static readonly Repository<TestEntity?> _repoWithNulls = [new("A"), null, new("BB"), new("CCC"), null];
    private static readonly Repository<TestEntity> _repoWithDuplicate = [new("CCC"), new("A"), new("BB"), new("CCC")];
    private static readonly Repository<TestEntity> _bigRepo = new(Enumerable.Range(0, 1000).ToArray(x => new TestEntity($"{x}")));
    private static readonly Repository<int> _emptyIntRepo = [];
    private static readonly Repository<int> _intRepo = [1, 2, 3, 4, 5, 6, 7, 8, 9];
    private static readonly Repository<int?> _emptyNullableIntRepo = [];
    private static readonly Repository<int?> _nullableIntRepo = [null, 2, null, 4, null, 6, null, 8, null];
    private static readonly Repository<int?> _allNullIntRepo = [null, null, null];

    private sealed class TestRepository : Repository<TestEntity>;
    private readonly TestRepository _updatableRepo = [new("A"), new("BB"), new("CCC")];

    private sealed class TestEntityEqualityComparer : IEqualityComparer<TestEntity> {
        public bool Equals(TestEntity? x, TestEntity? y)
            => ReferenceEquals(x, y)
            || (x is not null
             && y is not null
             && x.GetType() == y.GetType()
             && x.Name == y.Name);

        public int GetHashCode(TestEntity obj) => obj.Name.GetHashCode();
    }
    private static readonly TestEntityEqualityComparer _equalityComparer = new();

    private sealed class TestEntityComparer : IComparer<BaseTestEntity> {
        public int Compare(BaseTestEntity? x, BaseTestEntity? y)
            => ReferenceEquals(x, y) ? 0 : x is null ? -1 : y is null ? 1 : string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
    private static readonly TestEntityComparer _comparer = new();

    private sealed class DummyRepositoryStrategy() : RepositoryStrategy<TestEntity>;
    private sealed class DummyRepository()
        : Repository<DummyRepositoryStrategy, TestEntity>(new(), []);
    private static readonly DummyRepository _dummyRepository = [];

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
    public void Constructor_WithStrategy_CreatesRepository() {
        var subject = new Repository<TestEntity>(new DummyRepositoryStrategy());

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
    public void Constructor_WithNameAndStrategy_CreatesRepository() {
        var subject = new Repository<TestEntity>("MyRepository", new DummyRepositoryStrategy());

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
