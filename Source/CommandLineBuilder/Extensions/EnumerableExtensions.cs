namespace DotNetToolbox.CommandLineBuilder.Extensions;

internal static class EnumerableExtensions {
    public static T? FirstOrDefaultByName<T>(this IEnumerable<T> source, string name) where T : Token => source.FirstOrDefault(i => i.Is(name.Trim()));

    public static T? FirstOrDefaultByNameOrAlias<T>(this IEnumerable<T> source, string name) where T : Argument {
        name = name.Trim().TrimStart('-');
        return name.Length == 0 ? null : source.FirstOrDefault(i => i.Is(name) || i.Is(name[0]));
    }
}
