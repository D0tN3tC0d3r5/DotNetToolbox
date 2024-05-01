namespace DotNetToolbox.Data.Strategies;

public partial class RepositoryStrategyProviderTests {
    [Fact]
    public void RepositoryStrategyEntry_StrategyTypeSetter_Creates() {
        var subject = new RepositoryStrategyEntry(typeof(InMemoryValueObjectRepositoryStrategy<string>), () => new InMemoryValueObjectRepositoryStrategy<string>());

        subject = subject with {
            StrategyType = typeof(InMemoryValueObjectRepositoryStrategy<int>),
        };

        subject.StrategyType.Should().NotBeNull();
    }

    [Fact]
    public void RepositoryStrategyEntry_CreateSetter_Creates() {
        var subject = new RepositoryStrategyEntry(typeof(InMemoryValueObjectRepositoryStrategy<string>), () => new InMemoryValueObjectRepositoryStrategy<string>());

        subject = subject with {
            Create = () => new InMemoryValueObjectRepositoryStrategy<int>(),
        };

        subject.Create.Should().NotBeNull();
    }

    [Fact]
    public void TryAdd_ForNotRegisteredStrategy_AddsItem() {
        _provider.TryAdd<ValueObjectRepositoryStrategy>();

        var before = _provider.Entries.Count;

        _provider.TryAdd<ValueObjectRepositoryStrategy>();

        _provider.Entries.Count.Should().Be(before);
    }

    [Fact]
    public void TryAdd_ForRegisteredStrategy_AddsItem() {
        _provider.TryAdd<ValueObjectRepositoryStrategy>();

        var before = _provider.Entries.Count;

        _provider.TryAdd<NewValueObjectRepositoryStrategy>();

        _provider.Entries.Count.Should().Be(before + 1);
    }

    [Fact]
    public void TryAdd_ForDuplicatedStrategy_AddsItem() {
        _provider.TryAdd<ValueObjectRepositoryStrategy>();

        var action = () => _provider.TryAdd<DuplicatedValueObjectRepositoryStrategy>();

        action.Should().Throw<InvalidOperationException>();
    }
}
