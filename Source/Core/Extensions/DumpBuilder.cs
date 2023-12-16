namespace DotNetToolbox.Extensions;

internal record DumpBuilder {
    private readonly object? _name;
    private readonly object? _value;
    private readonly Type? _type;
    private readonly DumpOptions _options;
    private readonly StringBuilder _builder;
    private readonly HashSet<object> _ancestors;

    private byte _level;

    private DumpBuilder(byte level, object? name, Type? type, object? value, DumpOptions options, HashSet<object>? ancestors = null, StringBuilder? builder = null) {
        _value = value;
        _type = type;
        _options = options;
        _level = level;
        _name = name;
        _builder = builder ?? new StringBuilder();
        _ancestors = ancestors ?? new();
        if (value is not null) _ancestors.Add(value);
    }

    public static string Build(object? value, DumpOptions options) {
        var dumper = new DumpBuilder(0, null, value?.GetType(), value, options);
        dumper.AddType();
        dumper.AddFormattedValue();
        return dumper.Build();
    }

    public string Build() => _builder.ToString();

    private void AddType() {
        if (!_options.ShowType || _type is null) return;
        _builder.Append('(');
        _builder.Append(GetTypeName(_type));
        _builder.Append(')');
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
        if (value is null) _builder.Append("null");
        else if (_options.CustomFormatters.TryGetValue(value.GetType(), out var formatter)) _builder.Append(formatter(value));
        else if (TryFormatValue(value, _options, out var formattedValue)) _builder.Append(formattedValue);
        else if (value.GetType().IsPrimitive) _builder.Append(InvokeToStringWithCulture(value.GetType(), value, _options));
        else if (value is IDictionary dict) AddCollection("[", "]", dict.GetEnumerator(), TryAddKeyValuePair);
        else if (value is IEnumerable list) AddCollection("[", "]", list.GetEnumerator(), TryAddListItem);
        else AddCollection("{", "}", GetProperties(_value!.GetType()).GetEnumerator(), TryAddProperty);
    }

    private bool TryFormatValue(object value, DumpOptions options, out string formattedValue) {
        formattedValue = null!;
        switch (value) {
            case char: formattedValue = $"'{value}'"; return true;
            case bool b: formattedValue = b ? "true" : "false"; return true;
            case decimal v: formattedValue = v.ToString(options.Culture); return true;
            case string: formattedValue = $"\"{value}\""; return true;
            case DateTime: 
            case DateTimeOffset:
            case DateOnly:
            case TimeOnly: formattedValue = $"{value:O}"; return true;
            case TimeSpan: formattedValue = $"{value:c}"; return true;
            case Guid: formattedValue = $"{value:d}"; return true;
            case Type t: formattedValue = GetTypeName(t); return true;
            default: return false;
        }
    }

    private bool TryAddListItem(IEnumerator enumerator, int count)
        => TryAddMember(_level, _options.ShowListIndexes ? count : null, null, enumerator.Current);

    private bool TryAddKeyValuePair(IDictionaryEnumerator enumerator, int _)
        => TryAddMember(_level, enumerator.Key, null, enumerator.Value);

    private bool TryAddProperty(IEnumerator enumerator, int _) {
        var member = (PropertyInfo)enumerator.Current!;
        var type = member.PropertyType;
        var value = member.GetValue(_value);
        return TryAddMember(_level, member.Name, type, value);
    }

    private void AddCollection<TEnumerator>(string openTag, string closeTag, TEnumerator enumerator, Func<TEnumerator, int, bool> tryAddElement)
        where TEnumerator : IEnumerator {
        if (_level >= _options.MaxLevel - 1) {
            _builder.Append("...");
            return;
        }
        StartBlock(openTag);
        enumerator.Reset();
        var counter = 0;
        try {
            var hasNext = enumerator.MoveNext();
            while (hasNext) {
                var memberAdded = tryAddElement(enumerator, counter);
                hasNext = enumerator.MoveNext();
                if (!hasNext) break;
                if (!memberAdded) continue;
                counter++;
                _builder.Append(',');
                if (_options.IndentOutput) AddNewLine();
            }
        }
        finally {
            (enumerator as IDisposable)?.Dispose();
        }
        if (_options.IndentOutput) AddNewLine();
        EndBlock(closeTag);
    }

    private void StartBlock(string? prefix) {
        if (string.IsNullOrEmpty(prefix)) return;
        _builder.Append(prefix);
        if (_options.IndentOutput) AddNewLine();
        _level++;
    }

    private bool TryAddMember(byte level, object? memberName, Type? memberType, object? value) {
        if (level > _options.MaxLevel) return false;
        if (value is not null && _ancestors.Any(a => ReferenceEquals(a, value))) return false;
        if (_options.IndentOutput) AddIndentation();
        var dumper = new DumpBuilder(level, memberName, memberType, value, _options, _ancestors, _builder);
        dumper.AddFormattedName();
        dumper.AddType();
        dumper.AddFormattedValue();
        return true;
    }

    private void AddFormattedName() {
        if (_name is null) return;
        AddFormattedValue(_name);
        _builder.Append(':');
        if (_options.IndentOutput) _builder.Append(' ');
    }

    private void EndBlock(string? suffix) {
        if (string.IsNullOrEmpty(suffix)) return;
        _level--;
        if (_options.IndentOutput) AddIndentation();
        _builder.Append(suffix);
    }

    private static PropertyInfo[] GetProperties(IReflect type)
        => type.GetMembers(BindingFlags.Public | BindingFlags.Instance)
        .Where(m => m.MemberType is MemberTypes.Property)
        .OfType<PropertyInfo>()
        .ToArray();

    private void AddIndentation() {
        if (_options.UseTabs) _builder.Append('\t', _level);
        else _builder.Append(' ', _level * _options.IndentSize);
    }

    private void AddNewLine() => _builder.Append(Environment.NewLine);

    private static string InvokeToStringWithCulture(IReflect objectType, object value, DumpOptions options) {
        var methodInfo = objectType.GetMethod("ToString", BindingFlags.Instance | BindingFlags.Public, null, [typeof(CultureInfo)], null);
        return methodInfo is null
           ? value.ToString()!
           : methodInfo.Invoke(value, new object[] { options.Culture })!.ToString()!;
    }
}
