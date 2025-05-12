using DotNetToolbox.Data.DataSources;

namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Seed_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.Seed([new("D")]);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Seed_PopulatesRepository() {
        var subject = new InMemoryDataSource<TestEntity>();
        subject.Seed([new("A"), new("BB"), new("CCC")]);
        subject.Count().Should().Be(3);
    }
}
