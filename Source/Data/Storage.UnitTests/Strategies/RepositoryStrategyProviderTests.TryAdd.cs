namespace DotNetToolbox.Data.Strategies;

public partial class RepositoryStrategyProviderTests {
    [Fact]
    public void RepositoryStrategyEntry_StrategyTypeSetter_Creates() {
        var subject = new RepositoryStrategyEntry(typeof(InMemoryRepositoryStrategy<string>), () => new InMemoryRepositoryStrategy<string>());

        subject = subject with {
            StrategyType = typeof(InMemoryRepositoryStrategy<int>),
        };

        subject.StrategyType.Should().NotBeNull();
    }

    [Fact]
    public void RepositoryStrategyEntry_CreateSetter_Creates() {
        var subject = new RepositoryStrategyEntry(typeof(InMemoryRepositoryStrategy<string>), () => new InMemoryRepositoryStrategy<string>());

        subject = subject with {
            Create = () => new InMemoryRepositoryStrategy<int>(),
        };

        subject.Create.Should().NotBeNull();
    }

    [Fact]
    public void TryAdd_ForNotRegisteredStrategy_AddsItem() {
        _provider.TryAdd<RepositoryStrategy>();

        var before = _provider.Entries.Count;

        _provider.TryAdd<RepositoryStrategy>();

        _provider.Entries.Count.Should().Be(before);
    }

    [Fact]
    public void TryAdd_ForRegisteredStrategy_AddsItem() {
        _provider.TryAdd<RepositoryStrategy>();

        var before = _provider.Entries.Count;

        _provider.TryAdd<NewRepositoryStrategy>();

        _provider.Entries.Count.Should().Be(before + 1);
    }

    [Fact]
    public void TryAdd_ForDuplicatedStrategy_AddsItem() {
        _provider.TryAdd<RepositoryStrategy>();

        var action = () => _provider.TryAdd<DuplicatedRepositoryStrategy>();

        action.Should().Throw<InvalidOperationException>();
    }
}
