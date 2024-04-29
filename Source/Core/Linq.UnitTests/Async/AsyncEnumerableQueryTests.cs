namespace System.Linq.Async;

public partial class AsyncEnumerableQueryTests {
    private abstract record BaseTestEntity(string Name);
    private sealed record TestEntity(string       Name) : BaseTestEntity(Name);

    private static readonly AsyncEnumerableQuery<BaseTestEntity> _emptyRepo            = new([]);
    private static readonly AsyncEnumerableQuery<TestEntity>     _singleRepo           = new([new("A")]);
    private static readonly AsyncEnumerableQuery<TestEntity>     _repo                 = new([new("A"), new("BB"), new("CCC")]);
    private static readonly AsyncEnumerableQuery<TestEntity?>    _repoWithNulls        = new([new("A"), null, new("BB"), new("CCC"), null]);
    private static readonly AsyncEnumerableQuery<TestEntity>     _repoWithDuplicate    = new([new("CCC"), new("A"), new("BB"), new("CCC")]);
    private static readonly AsyncEnumerableQuery<TestEntity>     _bigRepo              = new(Enumerable.Range(0, 1000).ToArray(x => new TestEntity($"{x}")));
    private static readonly AsyncEnumerableQuery<int>            _emptyIntRepo         = new([]);
    private static readonly AsyncEnumerableQuery<int>            _intRepo              = new([1, 2, 3, 4, 5, 6, 7, 8, 9]);
    private static readonly AsyncEnumerableQuery<int?>           _emptyNullableIntRepo = new([]);
    private static readonly AsyncEnumerableQuery<int?>           _nullableIntRepo      = new([null, 2, null, 4, null, 6, null, 8, null]);
    private static readonly AsyncEnumerableQuery<int?>           _allNullIntRepo       = new([null, null, null]);

    private sealed class TestRepository() : AsyncEnumerableQuery<TestEntity>([]);

    private sealed class TestEntityEqualityComparer : IEqualityComparer<TestEntity> {
        public bool Equals(TestEntity? x, TestEntity? y)
            => ReferenceEquals(x, y)
            || (x is not null
             && y is not null
             && x.GetType() == y.GetType()
             && x.Name      == y.Name);

        public int GetHashCode(TestEntity obj) => obj.Name.GetHashCode();
    }
    private static readonly TestEntityEqualityComparer _equalityComparer = new();

    private sealed class TestEntityComparer : IComparer<BaseTestEntity> {
        public int Compare(BaseTestEntity? x, BaseTestEntity? y)
            => ReferenceEquals(x, y) ? 0 : x is null ? -1 : y is null ? 1 : string.Compare(x.Name, y.Name, StringComparison.Ordinal);
    }
    private static readonly TestEntityComparer _comparer = new();
}
