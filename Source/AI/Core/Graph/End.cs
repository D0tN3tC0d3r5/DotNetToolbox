namespace DotNetToolbox.AI.Graph;

public sealed record End<TValue>(TValue? Value = default)
    : NodeResult<TValue>(Value);
