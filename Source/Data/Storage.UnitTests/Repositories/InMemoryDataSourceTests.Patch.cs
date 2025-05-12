namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Patch_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.Patch(static _ => true, static _ => { });
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Patch_WithKey_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.Patch(0, static _ => { });
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Patch_ChangesItem() {
        _updatableRepo.Patch(static s => s.Name == "BB", static s => s.Name = "Z");
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public void Patch_NonExistingItem_DoesNothing() {
        _updatableRepo.Patch(static s => s.Name == "K", static s => s.Name = "Z");
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().BeNull();
    }
    [Fact]
    public void Patch_WithKey_ChangesItem() {
        _updatableRepo.Patch(2, static s => s.Name = "Z");
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public void Patch_WithKey_NonExistingItem_DoesNothing() {
        _updatableRepo.Patch(99, static s => s.Name = "Z");
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().BeNull();
    }
}
