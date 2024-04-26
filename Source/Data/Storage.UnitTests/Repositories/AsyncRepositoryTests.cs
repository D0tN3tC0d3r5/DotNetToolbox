namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    private abstract record BaseTestEntity(string Name);
    private sealed record TestEntity(string Name) : BaseTestEntity(Name);
    private readonly AsyncRepository<BaseTestEntity> _emptySet = [];
    private readonly AsyncRepository<TestEntity> _set1 = [new("A"), new("B"), new("C")];
    private readonly AsyncRepository<TestEntity> _set2 = [new("X"), new("Y"), new("Z")];

    private sealed class TestEntityEqualityComparer : IEqualityComparer<TestEntity> {
        public bool Equals(TestEntity? x, TestEntity? y)
            => ReferenceEquals(x, y)
            || (x is not null
             && y is not null
             && x.GetType() == y.GetType()
             && x.Name == y.Name);

        public int GetHashCode(TestEntity obj) => obj.Name.GetHashCode();
    }

    private sealed class TestEntityComparer : IComparer<TestEntity> {
        public int Compare(TestEntity? x, TestEntity? y)
            => ReferenceEquals(x, y) ? 0 : x is null ? -1 : y is null ? 1 : string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }

    private sealed class TestRepository : AsyncRepository<TestEntity>;

    private sealed class DummyAsyncRepositoryStrategy : AsyncRepositoryStrategy<TestEntity>;

    private readonly TestRepository _childRepo = [new("X"), new("Y"), new("Z")];

    private readonly RepositoryStrategyProvider _provider = new();

    public AsyncRepositoryTests() {
        _provider.TryAdd<DummyAsyncRepositoryStrategy>();
    }
}
