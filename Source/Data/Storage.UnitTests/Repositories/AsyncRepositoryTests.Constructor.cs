namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public void Constructor_WithStrategyFactory_CreatesRepository() {
        var subject = new AsyncRepository<TestEntity>(_provider);
        subject.Should().NotBeNull();
        subject.ElementType.Should().Be(typeof(TestEntity));
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
        subject.AsyncProvider.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithSeed_CreatesRepository() {
        var subject = new AsyncRepository<TestEntity>([new("A"), new("BB"), new("CCC")]);
        subject.Should().NotBeNull();
        subject.Count().Should().Be(3);
    }
}
