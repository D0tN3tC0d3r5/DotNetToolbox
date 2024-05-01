namespace DotNetToolbox.Data.Repositories;

public class RepositoryFactoryTests {
    private readonly RepositoryFactory _factory = new();

    [Fact]
    public void CreateRepository_ReturnsRepository() {
        var result = _factory.CreateRepository<ValueObjectRepository<int>, int>();

        result.Should().BeOfType<ValueObjectRepository<int>>();
    }

    [Fact]
    public void CreateRepository_WithStrategy_ReturnsRepository() {
        var result = _factory.CreateRepository<ValueObjectRepository<int>, int>(new InMemoryValueObjectRepositoryStrategy<int>());

        result.Should().BeOfType<ValueObjectRepository<int>>();
    }
}
