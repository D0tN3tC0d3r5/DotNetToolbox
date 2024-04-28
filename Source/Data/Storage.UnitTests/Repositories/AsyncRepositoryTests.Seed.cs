namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public void Seed_ForBaseStrategy_ShouldThrow() {
        var action = () => _dummyStrategy.Seed([]);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task SeedAsync_ForBaseStrategy_ShouldThrow() {
        var action = () => _dummyStrategy.SeedAsync(AsyncEnumerable.Empty<TestEntity>());
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Seed_PopulatesRepository() {
        var subject = new AsyncRepository<TestEntity>();
        subject.Seed([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }

    [Fact]
    public async Task SeedAsync_PopulatesRepository() {
        var subject = new AsyncRepository<TestEntity>();
        var seed = new AsyncEnumerable<TestEntity>([new("A"), new("BB"), new("CCC")]);
        await subject.SeedAsync(seed);
        subject.Count().Should().Be(3);
    }
}
