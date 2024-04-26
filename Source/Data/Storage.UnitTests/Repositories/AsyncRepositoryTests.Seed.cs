namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public void Seed_PopulatesRepository() {
        var subject = new AsyncRepository<TestEntity>();
        subject.Seed([new("A"), new("B"), new("C")]);
        subject.Count().Should().Be(3);
    }

    [Fact]
    public async Task SeedAsync_PopulatesRepository() {
        var subject = new AsyncRepository<TestEntity>();
        var seed = new AsyncEnumerable<TestEntity>([new("A"), new("B"), new("C")]);
        await subject.SeedAsync(seed);
        subject.Count().Should().Be(3);
    }
}
