namespace DotNetToolbox.Data.Repositories;

public class CreateOnlyRepositoryTests {
    private readonly CreateOnlyRepository<Test> _set;

    private class Test {
        public string Name { get; set; } = default!;
    };

    public CreateOnlyRepositoryTests() {
        var list = new List<Test> {
                                      new() { Name = "First" },
                                      new() { Name = "Second" },
                                  };
        _set = new(list);
    }

    [Fact]
    public void Create_ForInMemory_ReturnsCount() {
        var result = _set.Create(i => i.Name = "Third");
        _set.Count().Should().Be(3);
        result.Name.Should().Be("Third");
    }
}