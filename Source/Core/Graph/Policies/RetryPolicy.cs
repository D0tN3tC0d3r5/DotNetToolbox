namespace DotNetToolbox.Graph.Policies;

public class RetryPolicy
    : Policy {
    private readonly uint _maxRetries;
    private readonly TimeSpan[] _delays;
    private static readonly Random _random = Random.Shared;
    private const int _jiggleSizeInTicks = 100;
    private static readonly TimeSpan _jiggle = TimeSpan.FromTicks(_jiggleSizeInTicks);

    public RetryPolicy(uint maxRetries, TimeSpan[] delays) {
        if (delays.Length != maxRetries)
            throw new ArgumentException("The number of delays must match the number of retries.");

        _maxRetries = maxRetries;
        _delays = delays;
    }

    private static TimeSpan[] FillDelays(uint maxRetries, TimeSpan delay) {
        if (maxRetries == 0)
            return [];

        if (delay < _jiggle)
            throw new ArgumentOutOfRangeException(nameof(delay), $"The delay must be at least {_jiggleSizeInTicks} ticks.");

        var delays = new TimeSpan[maxRetries];
        for (var i = 0; i < maxRetries; i++) {
            delay.Add(TimeSpan.FromTicks(_random.Next(-_jiggleSizeInTicks, _jiggleSizeInTicks)));
            delays[i] = delay;
        }

        return delays;
    }

    public RetryPolicy(uint maxRetries, TimeSpan delay)
        : this(maxRetries, FillDelays(maxRetries, delay)) {
    }

    public override void Execute(Action action) {
        var attempts = 0;
        while (true) {
            try {
                action();
            }
            catch (Exception) {
                attempts++;
                if (attempts >= _maxRetries)
                    throw;

                Thread.Sleep(_delays[attempts - 1]);
            }
        }
    }
}
