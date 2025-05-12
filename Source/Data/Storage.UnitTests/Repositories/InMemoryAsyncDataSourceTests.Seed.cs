using DotNetToolbox.Data.DataSources;

namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task SeedAsync_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.SeedAsync([new("D")]);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Seed_PopulatesRepository() {
        var subject = new InMemoryDataSource<TestEntity>();
        subject.Seed([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }
}
