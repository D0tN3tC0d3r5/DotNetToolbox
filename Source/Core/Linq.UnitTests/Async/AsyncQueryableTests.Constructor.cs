namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public void Constructor_WithStrategyFactory_CreatesRepository() {
        IQueryable subject = new AsyncQueryable<TestEntity>([]);

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be<TestEntity>();
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithSeed_CreatesRepository() {
        var subject = new AsyncQueryable<TestEntity>([new("A"), new("BB"), new("CCC")]);
        subject.Should().NotBeNull();
        subject.Count().Should().Be(3);
    }
}
