namespace DotNetToolbox.Data.Repositories;

public partial class ValueObjectRepositoryTests {
    [Fact]
    public void Seed_PopulatesRepository() {
        var subject = new ValueObjectRepository<TestEntity>();
        subject.Seed([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }

    [Fact]
    public async Task SeedAsync_PopulatesRepository() {
        var subject = new ValueObjectRepository<TestEntity>();
        await subject.SeedAsync([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }
}
