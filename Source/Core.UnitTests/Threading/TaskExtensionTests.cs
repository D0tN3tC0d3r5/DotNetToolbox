namespace System.Threading;

public class TaskExtensionTests {
    private static readonly FieldInfo _taskAwaiterField = typeof(ConfiguredTaskAwaitable).GetField("m_configuredTaskAwaiter", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static readonly FieldInfo _taskContinueOnCapturedContextField = typeof(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter).GetField("m_continueOnCapturedContext", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static readonly FieldInfo _taskOfTAwaiterField = typeof(ConfiguredTaskAwaitable<int>).GetField("m_configuredTaskAwaiter", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static readonly FieldInfo _taskOfTContinueOnCapturedContextField = typeof(ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter).GetField("m_continueOnCapturedContext", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static readonly FieldInfo _valueTaskField = typeof(ConfiguredValueTaskAwaitable).GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static readonly FieldInfo _valueTaskContinueOnCapturedContextField = typeof(ValueTask).GetField("_continueOnCapturedContext", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static readonly FieldInfo _valueTaskOfTField = typeof(ConfiguredValueTaskAwaitable<int>).GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance)!;
    private static readonly FieldInfo _valueTaskOfTContinueOnCapturedContextField = typeof(ValueTask<int>).GetField("_continueOnCapturedContext", BindingFlags.NonPublic | BindingFlags.Instance)!;

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
        var onCancel = Substitute.For<Action<ValueTask, OperationCanceledException>>();

        // Act
        var act = () => TestValueTask().FireAndForget(onCancel);

        // Assert
        act.Should().NotThrow();
        onCancel.DidNotReceive().Invoke(Arg.Any<ValueTask>(), Arg.Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithValueTaskAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = Substitute.For<Action<ValueTask, Exception>>();

        // Act
        var act = () => TestValueTask().FireAndForget(onException);

        // Assert
        act.Should().NotThrow();
        onException.DidNotReceive().Invoke(Arg.Any<ValueTask>(), Arg.Any<Exception>());
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
        var onCancel = Substitute.For<Action<ValueTask, OperationCanceledException>>();

        // Act
        var act = () => TestCanceledValueTask().FireAndForget(onCancel);

        // Assert
        act.Should().NotThrow();
        onCancel.Received(1).Invoke(Arg.Any<ValueTask>(), Arg.Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithFailingValueTaskAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = Substitute.For<Action<ValueTask, Exception>>();

        // Act
        var act = () => TestFaultyValueTask().FireAndForget(onException);

        // Assert
        act.Should().NotThrow();
        onException.Received(1).Invoke(Arg.Any<ValueTask>(), Arg.Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithValueTaskOfT_ShouldDoNothing() {
        // Arrange
        var onResult = Substitute.For<Action<int>>();

        // Act
        var act = () => TestValueTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Arg.Any<int>());
    }

    [Fact]
    public void FireAndForget_WithValueTaskOfTAndOnCancel_ShouldCallOnException() {
        // Arrange
        var onResult = Substitute.For<Action<int>>();
        var onCancel = Substitute.For<Action<ValueTask<int>, OperationCanceledException>>();

        // Act
        var act = () => TestValueTaskOfT().FireAndForget(onResult, onCancel);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Arg.Any<int>());
        onCancel.DidNotReceive().Invoke(Arg.Any<ValueTask<int>>(), Arg.Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithValueTaskOfTAndOnException_ShouldCallOnException() {
        // Arrange
        var onResult = Substitute.For<Action<int>>();
        var onException = Substitute.For<Action<ValueTask<int>, Exception>>();

        // Act
        var act = () => TestValueTaskOfT().FireAndForget(onResult, onException);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Arg.Any<int>());
        onException.DidNotReceive().Invoke(Arg.Any<ValueTask<int>>(), Arg.Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithFailingValueTaskOfTAndNoOnException_ShouldDoNothing() {
        // Arrange
        var onResult = Substitute.For<Action<int>>();

        // Act
        var act = () => TestFaultyValueTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Arg.Any<int>());
    }

    [Fact]
    public void FireAndForget_WithCanceledValueTaskOfT_ShouldDoNothing() {
        // Arrange
        var onResult = Substitute.For<Action<int>>();

        // Act
        var act = () => TestCanceledValueTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Arg.Any<int>());
    }

    [Fact]
    public void FireAndForget_WithCanceledValueTaskOfTAndOnCancel_ShouldDoNothing() {
        // Arrange
        var onCancel = Substitute.For<Action<ValueTask<int>, OperationCanceledException>>();
        var onResult = Substitute.For<Action<int>>();

        // Act
        var act = () => TestCanceledValueTaskOfT().FireAndForget(onResult, onCancel);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Arg.Any<int>());
        onCancel.Received(1).Invoke(Arg.Any<ValueTask<int>>(), Arg.Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithFailingValueTaskOfTAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = Substitute.For<Action<ValueTask<int>, Exception>>();
        var onResult = Substitute.For<Action<int>>();

        // Act
        var act = () => TestFaultyValueTaskOfT().FireAndForget(onResult, onException);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Arg.Any<int>());
        onException.Received(1).Invoke(Arg.Any<ValueTask<int>>(), Arg.Any<Exception>());
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
        var onCancel = Substitute.For<Action<Task, OperationCanceledException>>();

        // Act
        var act = () => TestTask().FireAndForget(onCancel);

        // Assert
        act.Should().NotThrow();
        onCancel.DidNotReceive().Invoke(Arg.Any<Task>(), Arg.Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithTaskAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = Substitute.For<Action<Task, Exception>>();

        // Act
        var act = () => TestTask().FireAndForget(onException);

        // Assert
        act.Should().NotThrow();
        onException.DidNotReceive().Invoke(Arg.Any<Task>(), Arg.Any<Exception>());
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
        var onCancel = Substitute.For<Action<Task, OperationCanceledException>>();

        // Act
        var act = () => TestCanceledTask().FireAndForget(onCancel);

        // Assert
        act.Should().NotThrow();
        onCancel.Received(1).Invoke(Arg.Any<Task>(), Arg.Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithFailingTaskAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = Substitute.For<Action<Task, Exception>>();

        // Act
        var act = () => TestFaultyTask().FireAndForget(onException);

        // Assert
        act.Should().NotThrow();
        onException.Received(1).Invoke(Arg.Any<Task>(), Arg.Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithTaskOfT_ShouldDoNothing() {
        // Arrange
        var onResult = Substitute.For<Action<int>>();

        // Act
        var act = () => TestTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Arg.Any<int>());
    }

    [Fact]
    public void FireAndForget_WithTaskOfTAndOnCancel_ShouldCallOnException() {
        // Arrange
        var onResult = Substitute.For<Action<int>>();
        var onCancel = Substitute.For<Action<Task<int>, OperationCanceledException>>();

        // Act
        var act = () => TestTaskOfT().FireAndForget(onResult, onCancel);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Arg.Any<int>());
        onCancel.DidNotReceive().Invoke(Arg.Any<Task<int>>(), Arg.Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithTaskOfTAndOnException_ShouldCallOnException() {
        // Arrange
        var onResult = Substitute.For<Action<int>>();
        var onException = Substitute.For<Action<Task<int>, Exception>>();

        // Act
        var act = () => TestTaskOfT().FireAndForget(onResult, onException);

        // Assert
        act.Should().NotThrow();
        onResult.Received(1).Invoke(Arg.Any<int>());
        onException.DidNotReceive().Invoke(Arg.Any<Task<int>>(), Arg.Any<Exception>());
    }

    [Fact]
    public void FireAndForget_WithFailingTaskOfTAndNoOnException_ShouldDoNothing() {
        // Arrange
        var onResult = Substitute.For<Action<int>>();

        // Act
        var act = () => TestFaultyTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Arg.Any<int>());
    }

    [Fact]
    public void FireAndForget_WithCanceledTaskOfT_ShouldDoNothing() {
        // Arrange
        var onResult = Substitute.For<Action<int>>();

        // Act
        var act = () => TestCanceledTaskOfT().FireAndForget(onResult);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Arg.Any<int>());
    }

    [Fact]
    public void FireAndForget_WithCanceledTaskOfTAndOnCancel_ShouldDoNothing() {
        // Arrange
        var onCancel = Substitute.For<Action<Task<int>, OperationCanceledException>>();
        var onResult = Substitute.For<Action<int>>();

        // Act
        var act = () => TestCanceledTaskOfT().FireAndForget(onResult, onCancel);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Arg.Any<int>());
        onCancel.Received(1).Invoke(Arg.Any<Task<int>>(), Arg.Any<OperationCanceledException>());
    }

    [Fact]
    public void FireAndForget_WithFailingTaskOfTAndOnException_ShouldCallOnException() {
        // Arrange
        var onException = Substitute.For<Action<Task<int>, Exception>>();
        var onResult = Substitute.For<Action<int>>();

        // Act
        var act = () => TestFaultyTaskOfT().FireAndForget(onResult, onException);

        // Assert
        act.Should().NotThrow();
        onResult.DidNotReceive().Invoke(Arg.Any<int>());
        onException.Received(1).Invoke(Arg.Any<Task<int>>(), Arg.Any<Exception>());
    }

    [Fact]
    public void ResumeImmediately_ForValueTask_ShouldSetConfigureAwaitToFalse() {
        // Act
        var task = TestValueTask().ResumeImmediately();

        // Assert
        var valueTask = (ValueTask)_valueTaskField.GetValue(task)!;
        var continueOnCapturedContextField = (bool)_valueTaskContinueOnCapturedContextField.GetValue(valueTask)!;
        continueOnCapturedContextField.Should().BeFalse();
    }

    [Fact]
    public void ResumeCapturedContext_ForValueTask_ShouldSetConfigureAwaitToTrue() {
        // Act
        var task = TestValueTask().ResumeCapturedContext();

        // Assert
        var valueTask = (ValueTask)_valueTaskField.GetValue(task)!;
        var continueOnCapturedContextField = (bool)_valueTaskContinueOnCapturedContextField.GetValue(valueTask)!;
        continueOnCapturedContextField.Should().BeTrue();
    }

    [Fact]
    public void ResumeImmediately_ForValueTaskOfT_ShouldSetConfigureAwaitToFalse() {
        // Act
        var task = TestValueTaskOfT().ResumeImmediately();

        // Assert
        var valueTask = (ValueTask<int>)_valueTaskOfTField.GetValue(task)!;
        var continueOnCapturedContextField = (bool)_valueTaskOfTContinueOnCapturedContextField.GetValue(valueTask)!;
        continueOnCapturedContextField.Should().BeFalse();
    }

    [Fact]
    public void ResumeCapturedContext_ForValueTaskOfT_ShouldSetConfigureAwaitToTrue() {
        // Act
        var task = TestValueTaskOfT().ResumeCapturedContext();

        // Assert
        var valueTask = (ValueTask<int>)_valueTaskOfTField.GetValue(task)!;
        var continueOnCapturedContextField = (bool)_valueTaskOfTContinueOnCapturedContextField.GetValue(valueTask)!;
        continueOnCapturedContextField.Should().BeTrue();
    }

    [Fact]
    public void ResumeImmediately_ForTask_ShouldSetConfigureAwaitToFalse() {
        // Act
        var task = TestTask().ResumeImmediately();

        // Assert
        var awaiter = (ConfiguredTaskAwaitable.ConfiguredTaskAwaiter)_taskAwaiterField.GetValue(task)!;
        var continueOnCapturedContextField = (bool)_taskContinueOnCapturedContextField.GetValue(awaiter)!;
        continueOnCapturedContextField.Should().BeFalse();
    }

    [Fact]
    public void ResumeCapturedContext_ForTask_ShouldSetConfigureAwaitToTrue() {
        // Act
        var task = TestTask().ResumeCapturedContext();

        // Assert
        var awaiter = (ConfiguredTaskAwaitable.ConfiguredTaskAwaiter)_taskAwaiterField.GetValue(task)!;
        var continueOnCapturedContextField = (bool)_taskContinueOnCapturedContextField.GetValue(awaiter)!;
        continueOnCapturedContextField.Should().BeTrue();
    }

    [Fact]
    public void ResumeImmediately_ForTaskOfT_ShouldSetConfigureAwaitToFalse() {
        // Act
        var task = TestTaskOfT().ResumeImmediately();

        // Assert
        var awaiter = (ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter)_taskOfTAwaiterField.GetValue(task)!;
        var continueOnCapturedContextField = (bool)_taskOfTContinueOnCapturedContextField.GetValue(awaiter)!;
        continueOnCapturedContextField.Should().BeFalse();
    }

    [Fact]
    public void ResumeCapturedContext_ForTaskOfT_ShouldSetConfigureAwaitToTrue() {
        // Act
        var task = TestTaskOfT().ResumeCapturedContext();

        // Assert
        var awaiter = (ConfiguredTaskAwaitable<int>.ConfiguredTaskAwaiter)_taskOfTAwaiterField.GetValue(task)!;
        var continueOnCapturedContextField = (bool)_taskOfTContinueOnCapturedContextField.GetValue(awaiter)!;
        continueOnCapturedContextField.Should().BeTrue();
    }

    [Fact]
    public async Task ContinueSynchronouslyWith_ForTask_ExecutesSynchronously() {
        // Arrange
        var action = Substitute.For<Action<Task>>();

        // Act
        await TestTask().ContinueSynchronouslyWith(action);

        // Assert
        action.Received(1).Invoke(Arg.Any<Task>());
    }

    [Fact]
    public async Task ContinueSynchronouslyWith_ForTask_WithReturn_ExecutesSynchronously() {
        // Arrange
        var function = Substitute.For<Func<Task, int>>();
        function(Arg.Any<Task>()).Returns(42);

        // Act
        var result = await TestTask().ContinueSynchronouslyWith(function);

        // Assert
        function.Received(1).Invoke(Arg.Any<Task>());
        result.Should().Be(42);
    }

    [Fact]
    public async Task ContinueSynchronouslyWith_ForTaskOfT_ExecutesSynchronously() {
        // Arrange
        var task = Task.FromResult(42);
        var action = Substitute.For<Action<Task<int>>>();

        // Act
        await task.ContinueSynchronouslyWith(action);

        // Assert
        action.Received(1).Invoke(Arg.Any<Task<int>>());
    }

    [Fact]
    public async Task ContinueSynchronouslyWith_ForTaskOfT_WithReturn_ExecutesSynchronously() {
        // Arrange
        var task = Task.FromResult(42);
        var function = Substitute.For<Func<Task<int>, string>>();
        function(Arg.Any<Task<int>>()).Returns("42");

        // Act
        var result = await task.ContinueSynchronouslyWith(function);

        // Assert
        function.Received(1).Invoke(Arg.Any<Task<int>>());
        result.Should().Be("42");
    }

    [Fact]
    public async Task ContinueSynchronouslyWith_ForFaultyTask_DoesNothing() {
        // Arrange
        var action = Substitute.For<Action<Task>>();

        // Act
        var result = () => TestFaultyTask().ContinueSynchronouslyWith(action);

        // Assert
        await result.Should().ThrowAsync<TaskCanceledException>();
        action.DidNotReceive().Invoke(Arg.Any<Task>());
    }

    [Fact]
    public async Task ContinueSynchronouslyWith_ForCanceledTask_DoesNothing() {
        // Arrange
        var action = Substitute.For<Action<Task>>();

        // Act
        var result = () => TestCanceledTask().ContinueSynchronouslyWith(action);

        // Assert
        await result.Should().ThrowAsync<TaskCanceledException>();
        action.DidNotReceive().Invoke(Arg.Any<Task>());
    }
}
