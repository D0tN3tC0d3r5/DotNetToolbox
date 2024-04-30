namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void Constructor_WithStrategy_CreatesRepository() {
        var subject = new Repository<TestEntity>(new DummyRepositoryStrategy());

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be(typeof(TestEntity));
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
        subject.AsyncProvider.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithStrategyFactory_CreatesRepository() {
        var provider = new RepositoryStrategyProvider();
        provider.TryAdd<DummyRepositoryStrategy>();

        var subject = new Repository<TestEntity>(provider);

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be(typeof(TestEntity));
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
        subject.AsyncProvider.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithSeed_CreatesRepository() {
        var subject = new Repository<TestEntity>([new("A"), new("BB"), new("CCC")]);
        subject.Should().NotBeNull();
        subject.Count().Should().Be(3);
    }
}
