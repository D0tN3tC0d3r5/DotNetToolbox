using DotNetToolbox.Data.DataSources;

namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Seed_BaseStrategy_ShouldThrow() {
        var action = () => _dummyDataSource.Seed([new("D")]);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task SeedAsync_BaseStrategy_ShouldThrow() {
        var action = () => _dummyDataSource.SeedAsync([new("D")]);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Seed_PopulatesRepository() {
        var subject = new InMemoryDataSource<TestEntity, int>();
        subject.Seed([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }

    [Fact]
    public async Task SeedAsync_PopulatesRepository() {
        var subject = new InMemoryDataSource<TestEntity, int>();
        await subject.SeedAsync([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }
}
