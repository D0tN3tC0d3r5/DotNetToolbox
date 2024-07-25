namespace DotNetToolbox.Graph.Policies;

public abstract class RetryPolicy
    : Policy {
    public const byte DefaultMaximumRetries = 3;
    public static readonly TimeSpan DefaultDelay = TimeSpan.FromMilliseconds(50);
    public const ushort DefaultJiggleSizeInTicks = 100;

    private static readonly Random _random = Random.Shared;

    protected RetryPolicy(byte? maxRetries = null, TimeSpan? delay = null, ushort? jiggleSizeInTicks = null) {
        MaxRetries = maxRetries ?? DefaultMaximumRetries;
        delay ??= DefaultDelay;
        jiggleSizeInTicks ??= DefaultJiggleSizeInTicks;
        if (delay.Value.Ticks < jiggleSizeInTicks)
            throw new ArgumentOutOfRangeException(nameof(delay), $"The delay must be at least {DefaultJiggleSizeInTicks} ticks.");
        Delays = FillDelays(MaxRetries, delay.Value, jiggleSizeInTicks.Value);
    }

    public byte MaxRetries { get; }
    public IReadOnlyList<TimeSpan> Delays { get; }

    private static TimeSpan[] FillDelays(byte maxRetries, TimeSpan delay, ushort jiggle) {
        if (maxRetries is byte.MinValue)
            return [];

        var delays = new TimeSpan[maxRetries];
        for (var i = 0; i < maxRetries; i++) {
            delays[i] = delay.Add(TimeSpan.FromTicks(_random.NextInt64(-jiggle, jiggle)));
        }

        return delays;
    }

    public sealed override void Execute(Action action) {
        var attempts = 0;
        while (true) {
            attempts++;
            try {
                if (TryExecute(action))
                    return;
                HandleActionFailure();
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

    protected abstract bool TryExecute(Action action);
}
