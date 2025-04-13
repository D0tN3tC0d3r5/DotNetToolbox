namespace DotNetToolbox.Validation;

public partial class UserNameValidator
    : UserNameValidator<IMap>
    , IValidator<string> {
    internal static readonly Regex UserNameFormat = GetUserNameFormatRegex();

    [GeneratedRegex("^(?!.*[_.-]{2})(?=.{3,}$)[a-z0-9_][a-z0-9_.-]+[a-z0-9_]$",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.Compiled,
                    500)]
    private static partial Regex GetUserNameFormatRegex();
}

public class UserNameValidator<TContext>
    : IValidator<string, TContext>
    where TContext : class {
    public virtual bool IsValid(string value, TContext? context = null) => !string.IsNullOrWhiteSpace(value) && UserNameValidator.UserNameFormat.IsMatch(value);
}
