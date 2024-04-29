namespace DotNetToolbox.Data.Repositories;

public partial class AsyncEnumerableQueryTests {
    [Fact]
    public void Constructor_WithStrategyFactory_CreatesRepository() {
        IQueryable subject = new AsyncEnumerableQuery<TestEntity>([]);

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be(typeof(TestEntity));
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithSeed_CreatesRepository() {
        var subject = new AsyncEnumerableQuery<TestEntity>([new("A"), new("BB"), new("CCC")]);
        subject.Should().NotBeNull();
        subject.Count().Should().Be(3);
    }
}
