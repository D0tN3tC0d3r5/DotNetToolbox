﻿namespace DotNetToolbox.Threading;

public static class TaskExtensions {
    public static void FireAndForget(this ValueTask task)
        => HandleFireAndForget(task, null, default(Action<Exception>));
    public static void FireAndForget(this ValueTask task, Action<Exception> onException)
        => HandleFireAndForget(task, null, IsNotNull(onException));
    public static void FireAndForget(this ValueTask task, Action<OperationCanceledException> onCancel)
        => HandleFireAndForget(task, IsNotNull(onCancel), null);
    public static void FireAndForget(this ValueTask task, Action<OperationCanceledException> onCancel, Action<Exception> onException)
        => HandleFireAndForget(task, IsNotNull(onCancel), IsNotNull(onException));
    public static void FireAndForget(this ValueTask task, Action<ValueTask, Exception> onException)
        => HandleFireAndForget(task, null, IsNotNull(onException));
    public static void FireAndForget(this ValueTask task, Action<ValueTask, OperationCanceledException> onCancel)
        => HandleFireAndForget(task, IsNotNull(onCancel), null);
    public static void FireAndForget(this ValueTask task, Action<ValueTask, OperationCanceledException> onCancel, Action<ValueTask, Exception> onException)
        => HandleFireAndForget(task, IsNotNull(onCancel), IsNotNull(onException));

    public static void FireAndForget<TResult>(this ValueTask<TResult> task, Action<TResult> onResult)
        => HandleFireAndForget(task, IsNotNull(onResult), null, default(Action<Exception>));
    public static void FireAndForget<TResult>(this ValueTask<TResult> task, Action<TResult> onResult, Action<Exception> onException)
        => HandleFireAndForget(task, onResult, null, IsNotNull(onException));
    public static void FireAndForget<TResult>(this ValueTask<TResult> task, Action<TResult> onResult, Action<OperationCanceledException> onCancel)
        => HandleFireAndForget(task, onResult, IsNotNull(onCancel), null);
    public static void FireAndForget<TResult>(this ValueTask<TResult> task, Action<TResult> onResult, Action<OperationCanceledException> onCancel, Action<Exception> onException)
        => HandleFireAndForget(task, onResult, IsNotNull(onCancel), IsNotNull(onException));
    public static void FireAndForget<TResult>(this ValueTask<TResult> task, Action<TResult> onResult, Action<ValueTask<TResult>, Exception> onException)
        => HandleFireAndForget(task, IsNotNull(onResult), null, IsNotNull(onException));
    public static void FireAndForget<TResult>(this ValueTask<TResult> task, Action<TResult> onResult, Action<ValueTask<TResult>, OperationCanceledException> onCancel)
        => HandleFireAndForget(task, onResult, IsNotNull(onCancel), null);
    public static void FireAndForget<TResult>(this ValueTask<TResult> task, Action<TResult> onResult, Action<ValueTask<TResult>, OperationCanceledException> onCancel, Action<ValueTask<TResult>, Exception> onException)
        => HandleFireAndForget(task, onResult, IsNotNull(onCancel), IsNotNull(onException));

    public static void FireAndForget(this Task task)
        => HandleFireAndForget(task, null, default(Action<Exception>));
    public static void FireAndForget(this Task task, Action<Exception> onException)
        => HandleFireAndForget(task, null, IsNotNull(onException));
    public static void FireAndForget(this Task task, Action<OperationCanceledException> onCancel)
        => HandleFireAndForget(task, IsNotNull(onCancel), null);
    public static void FireAndForget(this Task task, Action<OperationCanceledException> onCancel, Action<Exception> onException)
        => HandleFireAndForget(task, IsNotNull(onCancel), IsNotNull(onException));
    public static void FireAndForget(this Task task, Action<Task, Exception> onException)
        => HandleFireAndForget(task, null, IsNotNull(onException));
    public static void FireAndForget(this Task task, Action<Task, OperationCanceledException> onCancel)
        => HandleFireAndForget(task, IsNotNull(onCancel), null);
    public static void FireAndForget(this Task task, Action<Task, OperationCanceledException> onCancel, Action<Task, Exception> onException)
        => HandleFireAndForget(task, IsNotNull(onCancel), IsNotNull(onException));

    public static void FireAndForget<TResult>(this Task<TResult> task, Action<TResult> onResult, Action<Exception> onException)
        => HandleFireAndForget(task, IsNotNull(onResult), null, IsNotNull(onException));
    public static void FireAndForget<TResult>(this Task<TResult> task, Action<TResult> onResult, Action<OperationCanceledException> onCancel)
        => HandleFireAndForget(task, IsNotNull(onResult), IsNotNull(onCancel), null);
    public static void FireAndForget<TResult>(this Task<TResult> task, Action<TResult> onResult, Action<OperationCanceledException> onCancel, Action<Exception> onException)
        => HandleFireAndForget(task, IsNotNull(onResult), IsNotNull(onCancel), IsNotNull(onException));
    public static void FireAndForget<TResult>(this Task<TResult> task, Action<TResult> onResult)
        => HandleFireAndForget(task, IsNotNull(onResult), null, default(Action<Exception>));
    public static void FireAndForget<TResult>(this Task<TResult> task, Action<TResult> onResult, Action<Task<TResult>, Exception> onException)
        => HandleFireAndForget(task, IsNotNull(onResult), null, IsNotNull(onException));
    public static void FireAndForget<TResult>(this Task<TResult> task, Action<TResult> onResult, Action<Task<TResult>, OperationCanceledException> onCancel)
        => HandleFireAndForget(task, IsNotNull(onResult), IsNotNull(onCancel), null);
    public static void FireAndForget<TResult>(this Task<TResult> task, Action<TResult> onResult, Action<Task<TResult>, OperationCanceledException> onCancel, Action<Task<TResult>, Exception> onException)
        => HandleFireAndForget(task, IsNotNull(onResult), IsNotNull(onCancel), IsNotNull(onException));

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

    private static async void HandleFireAndForget(ValueTask task, Action<OperationCanceledException>? onCancel, Action<Exception>? onException) {
        try {
            await IsNotNull(task).ConfigureAwait(false); // Fire and forget do not need to capture the current context
        }
        catch (OperationCanceledException ex) {
            onCancel?.Invoke(ex);
        }
        catch (Exception ex) {
            onException?.Invoke(ex);
        }
    }

    private static async void HandleFireAndForget(ValueTask task, Action<ValueTask, OperationCanceledException>? onCancel, Action<ValueTask, Exception>? onException) {
        try {
            await IsNotNull(task).ConfigureAwait(false); // Fire and forget do not need to capture the current context
        }
        catch (OperationCanceledException ex) {
            onCancel?.Invoke(task, ex);
        }
        catch (Exception ex) {
            onException?.Invoke(task, ex);
        }
    }

    private static async void HandleFireAndForget<TResult>(ValueTask<TResult> task, Action<TResult> onResult, Action<OperationCanceledException>? onCancel, Action<Exception>? onException) {
        try {
            var result = await task.ConfigureAwait(false); // Fire and forget do not need to capture the current context
            onResult(result);
        }
        catch (OperationCanceledException ex) {
            onCancel?.Invoke(ex);
        }
        catch (Exception ex) {
            onException?.Invoke(ex);
        }
    }

    private static async void HandleFireAndForget<TResult>(ValueTask<TResult> task, Action<TResult> onResult, Action<ValueTask<TResult>, OperationCanceledException>? onCancel, Action<ValueTask<TResult>, Exception>? onException) {
        try {
            var result = await task.ConfigureAwait(false); // Fire and forget do not need to capture the current context
            onResult(result);
        }
        catch (OperationCanceledException ex) {
            onCancel?.Invoke(task, ex);
        }
        catch (Exception ex) {
            onException?.Invoke(task, ex);
        }
    }

    private static async void HandleFireAndForget(Task task, Action<OperationCanceledException>? onCancel, Action<Exception>? onException) {
        try {
            await task.ConfigureAwait(false); // Fire and forget do not need to capture the current context
        }
        catch (OperationCanceledException ex) {
            onCancel?.Invoke(ex);
        }
        catch (Exception ex) {
            onException?.Invoke(ex);
        }
    }

    private static async void HandleFireAndForget(Task task, Action<Task, OperationCanceledException>? onCancel, Action<Task, Exception>? onException) {
        try {
            await task.ConfigureAwait(false); // Fire and forget do not need to capture the current context
        }
        catch (OperationCanceledException ex) {
            onCancel?.Invoke(task, ex);
        }
        catch (Exception ex) {
            onException?.Invoke(task, ex);
        }
    }

    private static async void HandleFireAndForget<TResult>(Task<TResult> task, Action<TResult> onResult, Action<OperationCanceledException>? onCancel, Action<Exception>? onException) {
        try {
            var result = await task.ConfigureAwait(false); // Fire and forget do not need to capture the current context
            onResult(result);
        }
        catch (OperationCanceledException ex) {
            onCancel?.Invoke(ex);
        }
        catch (Exception ex) {
            onException?.Invoke(ex);
        }
    }

    private static async void HandleFireAndForget<TResult>(Task<TResult> task, Action<TResult> onResult, Action<Task<TResult>, OperationCanceledException>? onCancel, Action<Task<TResult>, Exception>? onException) {
        try {
            var result = await task.ConfigureAwait(false); // Fire and forget do not need to capture the current context
            onResult(result);
        }
        catch (OperationCanceledException ex) {
            onCancel?.Invoke(task, ex);
        }
        catch (Exception ex) {
            onException?.Invoke(task, ex);
        }
    }
}
