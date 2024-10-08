﻿namespace DotNetToolbox.Graph.Policies;

public class RetryPolicy
    : RetryPolicy<RetryPolicy> {
    public const byte DefaultMaximumRetries = 3;
    public const ushort DefaultJiggleSizeInTicks = 100;
    public static readonly TimeSpan DefaultDelay = TimeSpan.FromMilliseconds(50);

    public RetryPolicy() {
    }

    public RetryPolicy(byte maxRetries, TimeSpan? delay = null, ushort? jiggleSizeInTicks = null)
        : base(maxRetries, delay, jiggleSizeInTicks) {
    }
}

public abstract class RetryPolicy<TPolicy>
    : IRetryPolicy,
      IHasDefault<TPolicy>
    where TPolicy : RetryPolicy<TPolicy>, new() {
    private readonly Random _random = Random.Shared;

    protected RetryPolicy()
        : this(RetryPolicy.DefaultMaximumRetries) {
    }

    public static TPolicy Default => new();

    protected RetryPolicy(byte maxRetries, TimeSpan? delay = null, ushort? jiggleSizeInTicks = null) {
        MaxRetries = maxRetries;
        delay ??= RetryPolicy.DefaultDelay;
        jiggleSizeInTicks ??= RetryPolicy.DefaultJiggleSizeInTicks;
        if (delay.Value.Ticks < jiggleSizeInTicks)
            throw new ArgumentOutOfRangeException(nameof(delay), $"The delay must be at least {RetryPolicy.DefaultJiggleSizeInTicks} ticks.");
        Delays = FillDelays(MaxRetries, delay.Value, jiggleSizeInTicks.Value);
    }

    public byte MaxRetries { get; }
    public IReadOnlyList<TimeSpan> Delays { get; }

    private TimeSpan[] FillDelays(byte maxRetries, TimeSpan delay, ushort jiggle) {
        if (maxRetries is byte.MinValue)
            return [];

        var delays = new TimeSpan[maxRetries];
        for (var i = 0; i < maxRetries; i++)
            delays[i] = delay.Add(TimeSpan.FromTicks(_random.NextInt64(-jiggle, jiggle)));

        return delays;
    }

    public async Task Execute(Func<Context, CancellationToken, Task> action, Context ctx, CancellationToken ct = default) {
        var attempts = 0;
        while (true) {
            attempts++;
            try {
                ct.ThrowIfCancellationRequested();
                if (await TryExecute(action, ctx, ct))
                    return;
                HandleActionFailure();
            }
            catch (OperationCanceledException) {
                throw;
            }
            catch (Exception ex) {
                HandleActionFailure(ex);
            }
        }

        void HandleActionFailure(Exception? ex = null) {
            if (MaxRetries != byte.MaxValue && attempts >= MaxRetries)
                throw new PolicyException($"The action failed to execute successfully. Maximum number of allowed retries: {MaxRetries}.", ex);

            Thread.Sleep(Delays[attempts - 1]);
        }
    }

    protected virtual async Task<bool> TryExecute(Func<Context, CancellationToken, Task> action, Context ctx, CancellationToken ct) {
        await action(ctx, ct);
        return true;
    }
}
