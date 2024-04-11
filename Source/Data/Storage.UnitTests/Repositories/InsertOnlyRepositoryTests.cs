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

    //[Fact]
    //public void Create_ForInMemory_Returns() {
    //    var result = _set.Create(i => { });

    //    result.Should().Be(4);
    //    _set.Count().Should().Be(4);
    //}
}
