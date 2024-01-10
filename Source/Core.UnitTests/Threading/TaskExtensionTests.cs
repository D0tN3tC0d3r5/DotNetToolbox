namespace DotNetToolbox.Threading;

public class TaskExtensionTests {
    private static readonly CancellationTokenSource _tokenSource = new();
    private static readonly CancellationToken _token = _tokenSource.Token;

    static TaskExtensionTests() {
        _tokenSource.Cancel();
    }

    private static Task TestTask() => Task.CompletedTask;
    private static Task TestFaultyTask() => Task.FromException(new InvalidOperationException());
    private static Task TestCanceledTask() => Task.FromCanceled(_token);
    private static Task<int> TestTaskOfT() => Task.FromResult(42);
    private static Task<int> TestFaultyTaskOfT() => Task.FromException<int>(new InvalidOperationException());
    private static Task<int> TestCanceledTaskOfT() => Task.FromCanceled<int>(_token);

    private static ValueTask TestValueTask() => ValueTask.CompletedTask;
    private static ValueTask TestFaultyValueTask() => ValueTask.FromException(new InvalidOperationException());
    private static ValueTask TestCanceledValueTask() => ValueTask.FromCanceled(_token);
    private static ValueTask<int> TestValueTaskOfT() => ValueTask.FromResult(42);
    private static ValueTask<int> TestFaultyValueTaskOfT() => ValueTask.FromException<int>(new InvalidOperationException());
    private static ValueTask<int> TestCanceledValueTaskOfT() => ValueTask.FromCanceled<int>(_token);

    [Fact]
    public void FireAndForget_WithValueTask_ShouldDoNothing() {
        // Act
        var act = () => TestValueTask().FireAndForget();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void FireAndForget_WithValueTaskAndOnCancel_ShouldCallOnException() {
        // Arrange
        var onCancel = For<Action<ValueTask, OperationCanceledException>>();

        // Act
        var act = () => TestValueTask().FireAndForget(onCancel);

        // Assert
        act.Should().NotThrow();
        onCancel.DidNotReceive().Invoke(Any<ValueTask>(), Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithValueTaskAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = For<Action<ValueTask, Exception>>();

        // Act
        var act = () => TestValueTask().FireAndForget(onException);

        // Assert
        act.Should().NotThrow();
        onException.DidNotReceive().Invoke(Any<ValueTask>(), Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithFailingValueTaskAndNoOnException_ShouldDoNothing() {
        // Act
        var act = () => TestFaultyValueTask().FireAndForget();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void FireAndForget_WithCanceledValueTask_ShouldDoNothing() {
        // Act
        var act = () => TestCanceledValueTask().FireAndForget();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void FireAndForget_WithCanceledValueTaskAndOnCancel_ShouldDoNothing() {
        // Arrange
        var onCancel = For<Action<ValueTask, OperationCanceledException>>();

        // Act
        var act = () => TestCanceledValueTask().FireAndForget(onCancel);

        // Assert
        act.Should().NotThrow();
        onCancel.Received(1).Invoke(Any<ValueTask>(), Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithFailingValueTaskAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = For<Action<ValueTask, Exception>>();

        // Act
        var act = () => TestFaultyValueTask().FireAndForget(onException);

        // Assert
        act.Should().NotThrow();
        onException.Received(1).Invoke(Any<ValueTask>(), Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithValueTaskOfT_ShouldDoNothing() {
        // Arrange
        var onResult = For<Action<int>>();

        // Act
        var act = () => TestValueTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Any<int>());
    }

    [Fact]
    public void FireAndForget_WithValueTaskOfTAndOnCancel_ShouldCallOnException() {
        // Arrange
        var onResult = For<Action<int>>();
        var onCancel = For<Action<ValueTask<int>, OperationCanceledException>>();

        // Act
        var act = () => TestValueTaskOfT().FireAndForget(onResult, onCancel);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Any<int>());
        onCancel.DidNotReceive().Invoke(Any<ValueTask<int>>(), Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithValueTaskOfTAndOnException_ShouldCallOnException() {
        // Arrange
        var onResult = For<Action<int>>();
        var onException = For<Action<ValueTask<int>, Exception>>();

        // Act
        var act = () => TestValueTaskOfT().FireAndForget(onResult, onException);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Any<int>());
        onException.DidNotReceive().Invoke(Any<ValueTask<int>>(), Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithFailingValueTaskOfTAndNoOnException_ShouldDoNothing() {
        // Arrange
        var onResult = For<Action<int>>();

        // Act
        var act = () => TestFaultyValueTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Any<int>());
    }

    [Fact]
    public void FireAndForget_WithCanceledValueTaskOfT_ShouldDoNothing() {
        // Arrange
        var onResult = For<Action<int>>();

        // Act
        var act = () => TestCanceledValueTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Any<int>());
    }

    [Fact]
    public void FireAndForget_WithCanceledValueTaskOfTAndOnCancel_ShouldDoNothing() {
        // Arrange
        var onCancel = For<Action<ValueTask<int>, OperationCanceledException>>();
        var onResult = For<Action<int>>();

        // Act
        var act = () => TestCanceledValueTaskOfT().FireAndForget(onResult, onCancel);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Any<int>());
        onCancel.Received(1).Invoke(Any<ValueTask<int>>(), Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithFailingValueTaskOfTAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = For<Action<ValueTask<int>, Exception>>();
        var onResult = For<Action<int>>();

        // Act
        var act = () => TestFaultyValueTaskOfT().FireAndForget(onResult, onException);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Any<int>());
        onException.Received(1).Invoke(Any<ValueTask<int>>(), Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithTask_ShouldDoNothing() {
        // Act
        var act = () => TestTask().FireAndForget();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void FireAndForget_WithTaskAndOnCancel_ShouldCallOnException() {
        // Arrange
        var onCancel = For<Action<Task, OperationCanceledException>>();

        // Act
        var act = () => TestTask().FireAndForget(onCancel);

        // Assert
        act.Should().NotThrow();
        onCancel.DidNotReceive().Invoke(Any<Task>(), Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithTaskAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = For<Action<Task, Exception>>();

        // Act
        var act = () => TestTask().FireAndForget(onException);

        // Assert
        act.Should().NotThrow();
        onException.DidNotReceive().Invoke(Any<Task>(), Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithFailingTaskAndNoOnException_ShouldDoNothing() {
        // Act
        var act = () => TestFaultyTask().FireAndForget();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void FireAndForget_WithCanceledTask_ShouldDoNothing() {
        // Act
        var act = () => TestCanceledTask().FireAndForget();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void FireAndForget_WithCanceledTaskAndOnCancel_ShouldDoNothing() {
        // Arrange
        var onCancel = For<Action<Task, OperationCanceledException>>();

        // Act
        var act = () => TestCanceledTask().FireAndForget(onCancel);

        // Assert
        act.Should().NotThrow();
        onCancel.Received(1).Invoke(Any<Task>(), Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithFailingTaskAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = For<Action<Task, Exception>>();

        // Act
        var act = () => TestFaultyTask().FireAndForget(onException);

        // Assert
        act.Should().NotThrow();
        onException.Received(1).Invoke(Any<Task>(), Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithTaskOfT_ShouldDoNothing() {
        // Arrange
        var onResult = For<Action<int>>();

        // Act
        var act = () => TestTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Any<int>());
    }

    [Fact]
    public void FireAndForget_WithTaskOfTAndOnCancel_ShouldCallOnException() {
        // Arrange
        var onResult = For<Action<int>>();
        var onCancel = For<Action<Task<int>, OperationCanceledException>>();

        // Act
        var act = () => TestTaskOfT().FireAndForget(onResult, onCancel);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Any<int>());
        onCancel.DidNotReceive().Invoke(Any<Task<int>>(), Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithTaskOfTAndOnException_ShouldCallOnException() {
        // Arrange
        var onResult = For<Action<int>>();
        var onException = For<Action<Task<int>, Exception>>();

        // Act
        var act = () => TestTaskOfT().FireAndForget(onResult, onException);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Any<int>());
        onException.DidNotReceive().Invoke(Any<Task<int>>(), Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithFailingTaskOfTAndNoOnException_ShouldDoNothing() {
        // Arrange
        var onResult = For<Action<int>>();

        // Act
        var act = () => TestFaultyTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Any<int>());
    }

    [Fact]
    public void FireAndForget_WithCanceledTaskOfT_ShouldDoNothing() {
        // Arrange
        var onResult = For<Action<int>>();

        // Act
        var act = () => TestCanceledTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Any<int>());
    }

    [Fact]
    public void FireAndForget_WithCanceledTaskOfTAndOnCancel_ShouldDoNothing() {
        // Arrange
        var onCancel = For<Action<Task<int>, OperationCanceledException>>();
        var onResult = For<Action<int>>();

        // Act
        var act = () => TestCanceledTaskOfT().FireAndForget(onResult, onCancel);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Any<int>());
        onCancel.Received(1).Invoke(Any<Task<int>>(), Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithFailingTaskOfTAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = For<Action<Task<int>, Exception>>();
        var onResult = For<Action<int>>();

        // Act
        var act = () => TestFaultyTaskOfT().FireAndForget(onResult, onException);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Any<int>());
        onException.Received(1).Invoke(Any<Task<int>>(), Any<Exception>());
    }
}
