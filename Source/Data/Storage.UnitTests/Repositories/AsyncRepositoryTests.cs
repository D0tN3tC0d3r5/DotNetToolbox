namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    private abstract record BaseTestEntity(string Name);
    private sealed record TestEntity(string Name) : BaseTestEntity(Name);
    private static readonly AsyncRepository<BaseTestEntity> _emptyRepo = [];
    private static readonly AsyncRepository<TestEntity> _singleRepo = [new("A")];
    private static readonly AsyncRepository<TestEntity> _repo = [new("A"), new("BB"), new("CCC")];
    private static readonly AsyncRepository<TestEntity?> _repoWithNulls = [new("A"), null, new("BB"), new("CCC"), null];
    private static readonly AsyncRepository<TestEntity> _repoWithDuplicate = [new("CCC"), new("A"), new("BB"), new("CCC")];
    private static readonly AsyncRepository<TestEntity> _bigRepo = new(Enumerable.Range(0, 1000).ToArray(x => new TestEntity($"{x}")));
    private static readonly AsyncRepository<int> _emptyIntRepo = [];
    private static readonly AsyncRepository<int> _intRepo = [1, 2, 3, 4, 5, 6, 7, 8, 9];
    private static readonly AsyncRepository<int?> _emptyNullableIntRepo = [];
    private static readonly AsyncRepository<int?> _nullableIntRepo = [null, 2, null, 4, null, 6, null, 8, null];
    private static readonly AsyncRepository<int?> _allNullIntRepo = [null, null, null];
    private readonly AsyncRepository<TestEntity> _updatableRepo = [new("A"), new("BB"), new("CCC")];

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

    private sealed class TestRepository : AsyncRepository<TestEntity>;

    private sealed class DummyAsyncRepositoryStrategy : AsyncRepositoryStrategy<TestEntity>;
    private readonly DummyAsyncRepositoryStrategy _dummyStrategy = [];

    private readonly TestRepository _childRepo = [new("X"), new("Y"), new("Z")];

    private readonly RepositoryStrategyProvider _provider = new();

    public AsyncRepositoryTests() {
        _provider.TryAdd<DummyAsyncRepositoryStrategy>();
    }
}
