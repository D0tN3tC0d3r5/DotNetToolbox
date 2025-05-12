namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Add_AddsItem() {
        _updatableRepo.Add(new("D"));
        _updatableRepo.Count().Should().Be(4);
    }
}
