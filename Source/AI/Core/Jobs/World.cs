﻿namespace DotNetToolbox.AI.Jobs;

public class World(IDateTimeProvider? dateTime = null)
    : Map,
      IValidatable,
      IWorld {
    private readonly IDateTimeProvider _dateTime = dateTime ?? DateTimeProvider.Default;

    public DateTimeOffset DateTime => _dateTime.Now;

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}
