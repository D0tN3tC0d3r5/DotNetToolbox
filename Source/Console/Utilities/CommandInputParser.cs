namespace DotNetToolbox.ConsoleApplication.Utilities;

internal static class CommandInputParser {
    private static readonly RegexOptions _parserOptions = IgnoreCase
                                                        | Singleline
                                                        | Compiled
                                                        | CultureInvariant
                                                        | NonBacktracking;
    private static readonly Regex _word = new("""
                                              "[^"]*"|\S+
                                              """, _parserOptions);
    public static string[] Parse(string input)
        => _word.Matches(input).Select(m => m.Value).ToArray();
}
