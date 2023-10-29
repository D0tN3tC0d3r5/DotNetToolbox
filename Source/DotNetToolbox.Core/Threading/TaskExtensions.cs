namespace System.Threading;

public static class TaskExtensions {
    public static void FireAndForget(this ValueTask task, Action<ValueTask, Exception>? onException = null)
        => FireAndForget(task, null, onException);
    public static void FireAndForget(this ValueTask task, Action<ValueTask, CancellationToken, Task?>? onCancel, Action<ValueTask, Exception>? onException = null)
        => HandleFireAndForget(task, onCancel, onException);
    public static void FireAndForget<TResult>(this ValueTask<TResult> task, Action<TResult> onResult, Action<ValueTask<TResult>, Exception>? onException = null)
        => FireAndForget(task, onResult, null, onException);
    public static void FireAndForget<TResult>(this ValueTask<TResult> task, Action<TResult> onResult, Action<ValueTask<TResult>, CancellationToken, Task?>? onCancel, Action<ValueTask<TResult>, Exception>? onException = null)
        => HandleFireAndForget(task, onResult, onCancel, onException);
    public static void FireAndForget(this Task task, Action<Task, Exception>? onException = null)
        => FireAndForget(task, null, onException);
    public static void FireAndForget(this Task task, Action<Task, CancellationToken, Task?>? onCancel, Action<Task, Exception>? onException = null)
        => HandleFireAndForget(task, onCancel, onException);
    public static void FireAndForget<TResult>(this Task<TResult> task, Action<TResult> onResult, Action<Task<TResult>, Exception>? onException = null)
        => FireAndForget(task, IsNotNull(onResult), null, onException);
    public static void FireAndForget<TResult>(this Task<TResult> task, Action<TResult> onResult, Action<Task<TResult>, CancellationToken, Task?>? onCancel, Action<Task<TResult>, Exception>? onException = null)
        => HandleFireAndForget(task, IsNotNull(onResult), onCancel, onException);

    public static ConfiguredValueTaskAwaitable ResumeCapturedContext(this ValueTask task)
        => task.ConfigureAwait(true);
    public static ConfiguredValueTaskAwaitable ResumeImmediately(this ValueTask task)
        => task.ConfigureAwait(false);
    public static ConfiguredValueTaskAwaitable<TResult> ResumeCapturedContext<TResult>(this ValueTask<TResult> task)
        => task.ConfigureAwait(true);
    public static ConfiguredValueTaskAwaitable<TResult> ResumeImmediately<TResult>(this ValueTask<TResult> task)
        => task.ConfigureAwait(false);
    public static ConfiguredTaskAwaitable ResumeCapturedContext(this Task task)
        => task.ConfigureAwait(true);
    public static ConfiguredTaskAwaitable ResumeImmediately(this Task task)
        => task.ConfigureAwait(false);
    public static ConfiguredTaskAwaitable<TResult> ResumeCapturedContext<TResult>(this Task<TResult> task)
        => task.ConfigureAwait(true);
    public static ConfiguredTaskAwaitable<TResult> ResumeImmediately<TResult>(this Task<TResult> task)
        => task.ConfigureAwait(false);

    public static Task ContinueSynchronouslyWith(this Task task, Action<Task> action)
        => task.ContinueWith(action,
                             CancellationToken.None,
                             TaskContinuationOptions.ExecuteSynchronously
                           | TaskContinuationOptions.NotOnCanceled
                           | TaskContinuationOptions.NotOnFaulted,
                             TaskScheduler.Default);
    public static Task<TResult> ContinueSynchronouslyWith<TResult>(this Task task, Func<Task, TResult> function)
        => task.ContinueWith(function,
                             CancellationToken.None,
                             TaskContinuationOptions.ExecuteSynchronously
                           | TaskContinuationOptions.NotOnCanceled
                           | TaskContinuationOptions.NotOnFaulted,
                             TaskScheduler.Default);
    public static Task ContinueSynchronouslyWith<TResult>(this Task<TResult> task, Action<Task<TResult>> action)
        => task.ContinueWith(action,
                             CancellationToken.None,
                             TaskContinuationOptions.ExecuteSynchronously
                           | TaskContinuationOptions.NotOnCanceled
                           | TaskContinuationOptions.NotOnFaulted,
                             TaskScheduler.Default);
    public static Task<TNewResult> ContinueSynchronouslyWith<TResult, TNewResult>(this Task<TResult> task, Func<Task<TResult>, TNewResult> function)
        => task.ContinueWith(function,
                             CancellationToken.None,
                             TaskContinuationOptions.ExecuteSynchronously
                           | TaskContinuationOptions.NotOnCanceled
                           | TaskContinuationOptions.NotOnFaulted,
                             TaskScheduler.Default);

    private static async void HandleFireAndForget(ValueTask task, Action<ValueTask, CancellationToken, Task?>? onCancel, Action<ValueTask, Exception>? onException) {
        try {
            await IsNotNull(task).ConfigureAwait(false); // Fire and forget do not need to capture the current context
        }
        catch (TaskCanceledException ex) {
            onCancel?.Invoke(task, ex.CancellationToken, ex.Task);
        }
        catch (Exception ex) {
            onException?.Invoke(task, ex);
        }
    }

    private static async void HandleFireAndForget<TResult>(ValueTask<TResult> task, Action<TResult> onResult, Action<ValueTask<TResult>, CancellationToken, Task?>? onCancel, Action<ValueTask<TResult>, Exception>? onException) {
        try {
            var result = await IsNotNull(task).ConfigureAwait(false); // Fire and forget do not need to capture the current context
            onResult(result);
        }
        catch (TaskCanceledException ex) {
            onCancel?.Invoke(task, ex.CancellationToken, ex.Task);
        }
        catch (Exception ex) {
            onException?.Invoke(task, ex);
        }
    }

    private static async void HandleFireAndForget(Task task, Action<Task, CancellationToken, Task?>? onCancel, Action<Task, Exception>? onException) {
        try {
            await IsNotNull(task).ConfigureAwait(false); // Fire and forget do not need to capture the current context
        }
        catch (TaskCanceledException ex) {
            onCancel?.Invoke(task, ex.CancellationToken, ex.Task);
        }
        catch (Exception ex) {
            onException?.Invoke(task, ex);
        }
    }

    private static async void HandleFireAndForget<TResult>(Task<TResult> task, Action<TResult> onResult, Action<Task<TResult>, CancellationToken, Task?>? onCancel, Action<Task<TResult>, Exception>? onException) {
        try {
            var result = await IsNotNull(task).ConfigureAwait(false); // Fire and forget do not need to capture the current context
            onResult(result);
        }
        catch (TaskCanceledException ex) {
            onCancel?.Invoke(task, ex.CancellationToken, ex.Task);
        }
        catch (Exception ex) {
            onException?.Invoke(task, ex);
        }
    }
}
