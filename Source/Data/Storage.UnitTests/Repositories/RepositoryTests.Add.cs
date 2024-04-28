namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void Add_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.Add(new("D"));
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Add_AddsItem() {
        _updatableRepo.Add(new("D"));
        _updatableRepo.Count().Should().Be(4);
    }
}
