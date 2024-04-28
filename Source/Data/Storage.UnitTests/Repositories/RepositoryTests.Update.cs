namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void Update_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.Update(_ => true, new(""));
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Update_ChangesItem() {
        _updatableRepo.Update(s => s.Name == "BB", new("Z"));
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }
}
