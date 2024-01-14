namespace DotNetToolbox;

public record struct Indexed<TValue>(int Index, TValue? Value);
