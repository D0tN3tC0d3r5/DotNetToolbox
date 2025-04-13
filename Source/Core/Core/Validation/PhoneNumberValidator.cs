namespace DotNetToolbox.Validation;

public partial class PhoneNumberValidator
    : PhoneNumberValidator<IMap>
    , IValidator<string> {
    internal static readonly Regex PhoneNumberFormat = GetPhoneNumberFormatRegex();

    [GeneratedRegex(@"^(?:\+\d{1,4})?(?:\s*\(\s*\d+\s*\)|\d+)(?:\s*\-?\d+)*$",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.Compiled,
                    500)]
    private static partial Regex GetPhoneNumberFormatRegex();
}

public class PhoneNumberValidator<TContext>
    : IValidator<string, TContext>
    where TContext : class {
    public virtual bool IsValid(string value, TContext? context = null)
        => !string.IsNullOrWhiteSpace(value) && PhoneNumberValidator.PhoneNumberFormat.IsMatch(value);
}
