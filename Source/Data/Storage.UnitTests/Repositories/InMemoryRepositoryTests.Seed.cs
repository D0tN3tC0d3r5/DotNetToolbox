namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryRepositoryTests {
    [Fact]
    public void Seed_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.Seed([new("D")]);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task SeedAsync_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.SeedAsync([new("D")]);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Seed_PopulatesRepository() {
        var subject = new InMemoryRepository<TestEntity, int>();
        subject.Seed([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }

    [Fact]
    public async Task SeedAsync_PopulatesRepository() {
        var subject = new InMemoryRepository<TestEntity, int>();
        await subject.SeedAsync([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }
}
