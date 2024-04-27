namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    private abstract record BaseTestEntity(string Name);
    private sealed record TestEntity(string Name) : BaseTestEntity(Name);
    private static readonly AsyncRepository<BaseTestEntity> _emptyRepo = [];
    private static readonly AsyncRepository<TestEntity> _singleRepo = [new("A")];
    private static readonly AsyncRepository<TestEntity> _bigRepo = new(Enumerable.Range(0, 1000).ToArray(x => new TestEntity($"{x}")));
    private readonly AsyncRepository<TestEntity> _repo = [new("A"), new("B"), new("C")];

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

    private sealed class TestEntityComparer : IComparer<TestEntity> {
        public int Compare(TestEntity? x, TestEntity? y)
            => ReferenceEquals(x, y) ? 0 : x is null ? -1 : y is null ? 1 : string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
    private static readonly TestEntityComparer _comparer = new();

    private sealed class TestRepository : AsyncRepository<TestEntity>;

    private sealed class DummyAsyncRepositoryStrategy : AsyncRepositoryStrategy<TestEntity>;

    private readonly TestRepository _childRepo = [new("X"), new("Y"), new("Z")];

    private readonly RepositoryStrategyProvider _provider = new();

    public AsyncRepositoryTests() {
        _provider.TryAdd<DummyAsyncRepositoryStrategy>();
    }
}
