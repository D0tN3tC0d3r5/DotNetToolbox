namespace DotNetToolbox.Data.Repositories;

public sealed partial class InMemoryDataSourceTests
    : IDisposable {
    private static readonly InMemoryDataSource<TestEntity> _readOnlyRepo = [.. Enumerable.Range(0, 90).Select(static x => new TestEntity((uint)(x + 1), $"{x}"))];
    private readonly InMemoryDataSource<TestEntity> _updatableRepo = [new(1, "A"), new(2, "BB"), new(3, "CCC")];

    private static readonly DummyDataSource _dummyDataSource = [];

    [Fact]
    public void Constructor_Default_CreatesDataSource() {
        var subject = new InMemoryDataSource<TestEntity>();

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be<TestEntity>();
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
        subject.Id.Should().Match("|>Data[TestEntity]_*<|");
    }

    public void Dispose() => _updatableRepo.Dispose();
}
