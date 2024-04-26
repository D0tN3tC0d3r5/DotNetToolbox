namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public void Constructor_WithStrategyFactory_CreatesRepository() {
        var subject = new AsyncRepository<TestEntity>(_provider);
        subject.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithSeed_CreatesRepository() {
        var subject = new AsyncRepository<TestEntity>([new("A"), new("B"), new("C")]);
        subject.Should().NotBeNull();
        subject.Count().Should().Be(3);
    }
}
