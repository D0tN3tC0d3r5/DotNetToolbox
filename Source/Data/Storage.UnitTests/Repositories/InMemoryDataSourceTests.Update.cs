namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Update_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.Update(static _ => true, new(""));
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Update_WithEntity_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.Update(new(""));
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Update_UpdatedItem() {
        _updatableRepo.Update(static s => s.Name == "BB", new("Z"));
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public void Update_ForInvalidItem_UpdatedItem() {
        _updatableRepo.Update(static s => s.Name == "ZZ", new("K"));
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "K").Should().BeNull();
    }

    [Fact]
    public void Update_WithEntity_UpdatedItem() {
        _updatableRepo.Update(new("Z") { Id = 2 });
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public void Update_WithEntity_ForInvalidItem_UpdatedItem() {
        _updatableRepo.Update(new("K") { Id = 99 });
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "K").Should().BeNull();
    }
}
