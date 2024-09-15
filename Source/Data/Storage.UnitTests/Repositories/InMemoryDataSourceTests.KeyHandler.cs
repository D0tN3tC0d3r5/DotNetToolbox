using DotNetToolbox.Data.DataSources;

namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Seed_WithCustomKeyHandler_PopulatesRepository() {
        var subject = new InMemoryDataSource<TestEntity>();
        subject.Seed([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }

    [Fact]
    public async Task SeedAsync_WithCustomKeyHandler_PopulatesRepository() {
        var subject = new InMemoryDataSource<TestEntity>();
        await subject.SeedAsync([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }
}
