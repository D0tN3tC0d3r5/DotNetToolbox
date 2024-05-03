namespace DotNetToolbox.Data.Strategies.Key;

public sealed class StringKeyHandler()
    : KeyHandler<string>(EqualityComparer<string>.Default);
