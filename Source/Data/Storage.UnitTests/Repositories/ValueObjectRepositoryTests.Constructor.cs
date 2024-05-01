namespace DotNetToolbox.Data.Repositories;

public partial class ValueObjectRepositoryTests {
    [Fact]
    public void Constructor_WithStrategy_CreatesRepository() {
        var subject = new ValueObjectRepository<TestEntity>(new DummyValueObjectRepositoryStrategy());

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be(typeof(TestEntity));
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
        subject.AsyncProvider.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithStrategyFactory_CreatesRepository() {
        var provider = new RepositoryStrategyProvider();
        provider.TryAdd<DummyValueObjectRepositoryStrategy>();

        var subject = new ValueObjectRepository<TestEntity>(provider);

        subject.Should().NotBeNull();
        subject.ElementType.Should().Be(typeof(TestEntity));
        subject.Expression.Should().NotBeNull();
        subject.Provider.Should().NotBeNull();
        subject.AsyncProvider.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithSeed_CreatesRepository() {
        var subject = new ValueObjectRepository<TestEntity>([new("A"), new("BB"), new("CCC")]);
        subject.Should().NotBeNull();
        subject.Count().Should().Be(3);
    }
}
