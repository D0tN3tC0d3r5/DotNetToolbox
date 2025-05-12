namespace DotNetToolbox.Data.Repositories;

public sealed partial class InMemoryAsyncDataSourceTests
    : IAsyncDisposable {
    private static readonly InMemoryAsyncDataSource<TestEntity> _readOnlyRepo = new([.. Enumerable.Range(0, 90).Select(static x => new TestEntity((uint)(x + 1), $"{x}"))]);
    private readonly InMemoryAsyncDataSource<TestEntity> _updatableRepo = new([new(1, "A"), new(2, "BB"), new(3, "CCC")]);

    private static readonly DummyAsyncDataSource _dummyDataSource = [];

    [Fact]
    public void Constructor_Default_CreatesRepository() {
        var subject = new InMemoryAsyncDataSource<TestEntity>();

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be<TestEntity>();
        subject.Expression.Should().NotBeNull();
        subject.AsyncProvider.Should().NotBeNull();
        subject.Id.Should().Match("|>Data[TestEntity]_*<|");
    }

    public ValueTask DisposeAsync()
        => _updatableRepo.DisposeAsync();
}
