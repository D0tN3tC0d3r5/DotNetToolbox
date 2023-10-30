namespace System.Validation.Commands;

public sealed class IsEqualToCommand
    : ValidationCommand{
    public IsEqualToCommand(object value, string source)
        : base(source) {
        ValidateAs = o => IsEquivalent(value, o);
        ValidationErrorMessage = MustBeEqualTo;
        GetErrorMessageArguments = o => new object?[] { GetString(value), GetString(o) };
    }

    private static string GetString(object? o) {
        if (o is not IEnumerable oc) return $"{o}";
        var sb = new StringBuilder();
        sb.Append('[');
        foreach (var i in oc) {
            sb.Append(GetString(i));
            sb.Append(", ");
        }

        sb.Remove(sb.Length - 2, 2);
        sb.Append(']');
        return sb.ToString();
    }

    private static bool IsEquivalent(object v, object? o)
    {
        if (v.Equals(o)) return true;
        if (v is not ICollection vc) return false;
        if (o is not ICollection oc) return false;
        if (oc.Count != vc.Count) return false;
        var d1 = GroupByCount(oc);
        var d2 = GroupByCount(vc);
        if (d1.Count != d2.Count) return false;
        foreach (var k1 in d1.Keys)
        {
            var k2 = d2.Keys.FirstOrDefault(k => IsEquivalent(k1, k));
            if (k2 is null) return false;
            if (d2[k2] != d1[k1]) return false;
        }

        return true;
    }

    private static Dictionary<object, int> GroupByCount(IEnumerable oc)
    {
        var d1 = new Dictionary<object, int>();
        foreach (var i in oc) {
            if (d1.TryGetValue(i, out var value)) d1[i] = ++value;
            else d1.Add(i, 1);
        }

        return d1;
    }
}
