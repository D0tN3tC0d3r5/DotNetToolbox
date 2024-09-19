using DotNetToolbox.Data.DataSources;
using DotNetToolbox.Data.Storages;
using DotNetToolbox.Results;

namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    private sealed class TestEntity()
        : IEntity<int> {
        public TestEntity(string name) : this() {
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public Result Validate(IMap? context = null) => Result.Success();
    };

    private static readonly InMemoryDataSource<TestEntity, int> _readOnlyRepo = [.. Enumerable.Range(0, 90).Select(x => new TestEntity($"{x}"))];
    private readonly InMemoryDataSource<TestEntity, int> _updatableRepo = [new("A"), new("BB"), new("CCC")];

    private sealed class DummyDataSource : DataSource<DummyStorage, TestEntity, int>;
    private sealed class DummyStorage
        : Storage<DummyStorage, TestEntity, int>;
    private static readonly DummyDataSource _dummyDataSource = [];

    [Fact]
    public void Constructor_Default_CreatesRepository() {
        var subject = new InMemoryDataSource<TestEntity>();

        subject.Should().NotBeNull();
        subject.ElementType.Should().BeOfType<TestEntity>();
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
        subject.AsyncProvider.Should().NotBeNull();
        subject.Id.Should().Match("|>Data[TestEntity]_*<|");
    }
}
