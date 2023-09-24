namespace DotNetToolbox.Utilities;

public partial record struct Base64Guid(Guid Value)
{
    public Base64Guid(string? value)
        : this(ToGuid(value))
    { }

    public static implicit operator Base64Guid(string value) => new(value);
    public static implicit operator Base64Guid(Guid value) => new(value);
    public static implicit operator string(Base64Guid value) => ToBase64(value.Value);
    public static implicit operator Guid(Base64Guid value) => value.Value;

    public static Base64Guid Parse(string? value) => new(value);

    public static bool TryParse(string? value, out Base64Guid result)
    {
        try
        {
            result = Parse(value);
            return true;
        }
        catch
        {
            result = default;
            return false;
        }
    }

    private static string ToBase64(Guid guid)
        => guid == Guid.Empty
            ? string.Empty
            : Convert.ToBase64String(guid.ToByteArray())
                     .TrimEnd('=')
                     .Replace('+', '-')
                     .Replace('/', '.');

    private static readonly Regex _base64CGuidFormat = Base64GuidFormat();

    private static Guid ToGuid(string? input)
    {
        if (input is null)
        {
            return Guid.Empty;
        }

        var text = IsNotNullOrWhiteSpace(input).Trim();
        if (!_base64CGuidFormat.IsMatch(text))
        {
            return new(input);
        }

        text = text
              .Replace('.', '/')
              .Replace('-', '+')
             + "==";

        var buffer = Convert.FromBase64String(text);
        return new(buffer);
    }

    [GeneratedRegex("^[a-zA-Z0-9\\-\\.]{22}$", RegexOptions.Compiled)]
    private static partial Regex Base64GuidFormat();
}
