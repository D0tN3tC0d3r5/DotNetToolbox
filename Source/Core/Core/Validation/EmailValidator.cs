namespace DotNetToolbox.Validation;

public partial class EmailValidator
    : EmailValidator<IMap>
    , IValidator<string> {
    internal static readonly Regex EmailFormat = GetEmailFormatRegex();

    [GeneratedRegex(@"^(?:[a-z0-9_]+(?:(?:\.|\-)[a-z0-9_]+)*)(?:\+[a-z0-9_]+(?:(?:\.|\-)[a-z0-9_]+)*)?\@(?:[a-z0-9]+(?:(?:\.|\-)[a-z0-9]+)*\.[a-z]{2,})$",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.Compiled,
                    500)]
    private static partial Regex GetEmailFormatRegex();
}

public class EmailValidator<TContext>
    : IValidator<string, TContext>
    where TContext : class {
    public virtual bool IsValid(string value, TContext? context = null)
        => !string.IsNullOrWhiteSpace(value) && EmailValidator.EmailFormat.IsMatch(value);
}
