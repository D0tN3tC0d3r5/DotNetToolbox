namespace DotNetToolbox.Utilities;

public class DisposableStateHolderTests
{
    [Fact]
    public void Dispose_ForDefaultStateType_CallsDisposeOnState() {
        // Arrange
        var disposable = For<IDisposable>();
        var holder = new DisposableStateHolder(disposable);

        // Act
        holder.Dispose();

        // Assert
        disposable.Received(1).Dispose();
    }

    [Fact]
    public void Dispose_WhenStateIsDisposable_CallsDisposeOnState()
    {
        // Arrange
        var disposable = For<IDisposable>();
        var holder = new DisposableStateHolder<IDisposable>(disposable);

        // Act
        holder.Dispose();

        // Assert
        disposable.Received(1).Dispose();
    }

    [Fact]
    public void Dispose_WhenStateIsNotDisposable_DoesNotThrow()
    {
        // Arrange
        var holder = new DisposableStateHolder<int>(42);

        // Act
        var act = holder.Dispose;

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void State_WhenCalled_ReturnsInitialState()
    {
        // Arrange
        const string state = "test";
        var holder = new DisposableStateHolder<string>(state);

        // Act
        var result = holder.State;

        // Assert
        result.Should().Be(state);
    }
}
