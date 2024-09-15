using DotNetToolbox.Data.DataSources;
using DotNetToolbox.Data.Storages;

namespace DotNetToolbox.Data.Repositories;

public class DataSourceFactoryTests {
    private readonly DataSourceFactory _factory = new();

    [Fact]
    public void CreateRepository_ReturnsRepository() {
        var result = _factory.CreateInMemory<int>();

        result.Should().BeOfType<InMemoryDataSource<int>>();
    }

    [Fact]
    public void CreateRepository_WithData_ReturnsSeededRepository() {
        var result = _factory.CreateInMemory([1, 2, 3]);

        result.Should().BeOfType<InMemoryDataSource<int>>();
        result.Count().Should().Be(3);
    }

    [Fact]
    public void CreateRepository_WithStrategy_ReturnsRepository() {
        var result = _factory.CreateFromStorage<InMemoryStorage<int>, int>();

        result.Should().BeOfType<DataSource<InMemoryStorage<int>, int>>();
    }

    [Fact]
    public void CreateRepository_WithStrategyAndData_ReturnsSeededRepository() {
        var result = _factory.CreateFromStorage<InMemoryStorage<int>, int>(data: [1, 2, 3]);

        result.Should().BeOfType<DataSource<InMemoryStorage<int>, int>>();
        result.Count().Should().Be(3);
    }
}
