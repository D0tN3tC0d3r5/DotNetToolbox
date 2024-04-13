namespace DotNetToolbox.Data.Repositories;

public class AddOrCreateOnlyRepositoryTests {
    private readonly AddOrCreateOnlyRepository<Test> _set;

    private class Test {
        public string Name { get; set; } = default!;
    };

    public AddOrCreateOnlyRepositoryTests() {
        var list = new List<Test> {
                                      new() { Name = "First" },
                                      new() { Name = "Second" },
                                  };
        _set = new(list);
    }

    [Fact]
    public void Add_ForInMemory_ReturnsCount() {
        var item = new Test() { Name = "Other" };
        _set.Add(item);
        _set.Count().Should().Be(3);
        _set.ElementAt(2).Name.Should().Be("Other");
    }

    [Fact]
    public void Create_ForInMemory_ReturnsCount() {
        var result = _set.Create(i => i.Name = "Third");
        _set.Count().Should().Be(3);
        result.Name.Should().Be("Third");
    }
}