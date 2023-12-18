using Member = (object? Name, System.Type? Type, object? Value);

namespace DotNetToolbox;

internal record DumpBuilder {
    private readonly object? _name;
    private readonly object? _value;
    private readonly Type? _type;
    private readonly DumpBuilderOptions _options;
    private readonly StringBuilder _builder;
    private readonly HashSet<object> _ancestors;

    private readonly byte _level;

    private DumpBuilder(byte level, object? name, Type? type, object? value, DumpBuilderOptions options, HashSet<object>? ancestors = null) {
        _value = value;
        _type = type;
        _options = options;
        _level = level;
        _name = name;
        _builder = new();
        _ancestors = ancestors ?? [];
        if (value is not null) _ancestors.Add(value);
    }

    public static string Build(object? value, DumpBuilderOptions options) {
        var dumper = new DumpBuilder(0, null, value?.GetType(), value, options);
        dumper.AddType();
        dumper.AddFormattedValue();
        return dumper.Build();
    }

    public string Build() => _builder.ToString();

    private void AddType() {
        if (_options.Layout == Layout.Json || _type is null) return;
        _builder.Append('<');
        _builder.Append(GetTypeName(_type));
        _builder.Append('>');
        AddSpacer();
    }

    public string GetTypeName(Type type) {
        var typeName = _options.ShowTypeFullName ? type.FullName! : type.Name;
        if (!type.IsGenericType) return typeName;
        var genericArguments = type.GetGenericArguments();
        var genericTypeName = typeName[..typeName.IndexOf('`')];
        var genericArgumentsNames = string.Join(", ", genericArguments.Select(GetTypeName));
        return $"{genericTypeName}<{genericArgumentsNames}>";
    }

    private void AddFormattedValue() => AddFormattedValue(_value);

    private void AddFormattedValue(object? value) {
        if (TryUseCustomFormatter(value)) return;
        if (TryUseDefaultFormatter(value)) return;
        AddFormattedComplexType(value);
    }

    private bool TryUseCustomFormatter(object? value) {
        var formattedValue = value is not null
            && _options.CustomFormatters.TryGetValue(value.GetType(), out var formatter)
                   ? formatter(value)
                   : null;
        if (formattedValue is null) return false;

        _builder.Append(formattedValue);
        return true;
    }

    private bool TryUseDefaultFormatter([NotNullWhen(false)] object? value) {
        var formattedValue = value switch {
            null => "null",
            bool => $"{value}".ToLower(),
            char => $"'{value}'",
            string => $"\"{value}\"",
            IConvertible v => v.ToString(_options.Culture),
            _ => null,
        };
        if (formattedValue is null) return false;

        _builder.Append(formattedValue);
        return true;
    }

    private void AddFormattedComplexType(object? value) {
        if (MaxLevelReached()) return;

        StartBlock();
        var enumerator = GetEnumerator(value);
        try {
            AddMembers(value, enumerator: enumerator);
        }
        finally {
            (enumerator as IDisposable)?.Dispose();
        }
        AddNewLine();
        EndBlock();
    }

    [MustDisposeResource]
    private IEnumerator GetEnumerator(object? value) => value is IEnumerable list
                                                            ? list.GetEnumerator()
                                                            : GetProperties(_value!.GetType()).GetEnumerator();

    private static PropertyInfo[] GetProperties(IReflect type)
        => type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToArray();

    private bool MaxLevelReached() {
        if (_options.MaxLevel == 0 || _level < _options.MaxLevel - 1) return false;
        _builder.Append("...");
        return true;

    }

    private void AddMembers(object? value, IEnumerator enumerator) {
        var counter = 0;
        var hasNext = enumerator.MoveNext();
        while (hasNext) {
            hasNext = AddMember(counter, value, enumerator);
            counter++;
        }
    }

    private bool AddMember(int counter, object? value, IEnumerator enumerator) {
        var memberFound = value is IEnumerable
            ? TryGetElement(enumerator, counter, out var member)
            : TryGetProperty(enumerator, out member);
        var memberAdded = memberFound && TryAddValue(member);
        var hasNext = enumerator.MoveNext();
        if (!hasNext || !memberAdded) return hasNext;

        _builder.Append(',');
        AddNewLine();
        return true;
    }

    private bool TryGetElement(IEnumerator enumerator, int count, out Member member) {
        member = new(0, null, null);
        try {
            if (enumerator is IDictionaryEnumerator dict) {
                member = new(dict.Key, null, dict.Value);
                return true;
            }

            var index = _options.ShowListIndexes ? count : (int?)null;
            var item = enumerator.Current;
            if (item is not null && _ancestors.Any(a => ReferenceEquals(a, item))) return false;
            member = new(index, null, item);
            return true;
        }
        catch (Exception) {
            return false;
        }
    }


    private bool TryGetProperty(IEnumerator enumerator, out Member member) {
        member = new(0, null, null);
        var prop = (PropertyInfo)enumerator.Current!;
        try {
            member = new(prop.Name, prop.PropertyType, prop.GetValue(_value));
            return true;
        }
        catch (Exception ex) {
            return false;
        }
    }

    private void StartBlock() {
        _builder.Append(_value is IEnumerable ? '[' : '{');
        AddNewLine();
    }

    private bool TryAddValue(Member member) {
        if (_level >= _options.MaxLevel) return false;
        if (member.Value is not null && _ancestors.Any(a => ReferenceEquals(a, member.Value))) return false;

        var valueDumper = new DumpBuilder((byte)(_level + 1), member.Name, member.Type, member.Value, _options, _ancestors);
        valueDumper.AddIndentation();
        valueDumper.AddFormattedName();
        valueDumper.AddType();
        valueDumper.AddFormattedValue();
        _builder.Append(valueDumper._builder);
        return true;
    }

    private void AddFormattedName() {
        if (_name is null) return;
        AddFormattedValue(_name);
        _builder.Append(':');
        AddSpacer();
    }

    private void AddSpacer() {
        if (!_options.Indented) return;
        _builder.Append(' ');
    }

    private void EndBlock() {
        AddIndentation();
        _builder.Append(_value is IEnumerable ? ']' : '}');
    }

    private void AddIndentation() {
        if (!_options.Indented) return;
        if (_options.UseTabs) _builder.Append('\t', _level);
        else _builder.Append(' ', _level * _options.IndentSize);
    }

    private void AddNewLine() {
        if (!_options.Indented) return;
        _builder.Append(Environment.NewLine);
    }
}
