namespace DotNetToolbox.Data.Repositories;

public class AddOnlyRepositoryTests {
    private readonly AddOnlyRepository<int> _set;

    public AddOnlyRepositoryTests() {
        var list = new List<int> { 1, 2, 3 };
        _set = new(list);
    }

    [Fact]
    public void Add_ForInMemory_ReturnsCount() {
        _set.Add(4);
        _set.Count().Should().Be(4);
    }
}

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
