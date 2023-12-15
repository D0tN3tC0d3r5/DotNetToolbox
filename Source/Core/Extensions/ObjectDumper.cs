namespace DotNetToolbox.Extensions;

internal record ObjectDumper {
    private readonly object? _memberName;
    private readonly object? _value;
    private readonly Type _valueType;
    private readonly DumperOptions _options;
    private byte _level;
    private readonly StringBuilder _builder;

    private ObjectDumper(byte level, object? memberName, object? value, DumperOptions options, StringBuilder? builder = null) {
        _value = value;
        _valueType = value?.GetType() ?? typeof(void);
        _options = options;
        _level = level;
        _memberName = memberName;
        _builder = builder ?? new StringBuilder();
    }

    public static string Dump(byte level, object? memberName, object? value, DumperOptions options, StringBuilder? builder) {
        if (level >= options.MaxLevel) return string.Empty;
        var dumper = new ObjectDumper(level, memberName, value, options, builder);
        dumper.AddValue();
        return dumper.Build();
    }

    private static void Execute(byte level, object? memberName, object? value, DumperOptions options, StringBuilder? builder) {
        if (level >= options.MaxLevel) return;
        var dumper = new ObjectDumper(level, memberName, value, options, builder);
        dumper.AddMemberName();
        dumper.AddValue();
    }

    public string Build() => _builder.ToString();

    private void AddMemberName() {
        if (_memberName is null) return;
        Execute(_level, null, _memberName, _options, _builder);
        _builder.Append(':');
        if (_options.IndentOutput) _builder.Append(' ');
    }

    private void AddValue() {
        if (_value is null) _builder.Append("null");
        else if (_options.CustomFormatters.TryGetValue(_valueType, out var formatter)) _builder.Append(formatter(_value));
        else if (TryFormatValue(_value, _options, out var formattedValue)) _builder.Append(formattedValue);
        else if (_valueType.IsPrimitive) _builder.Append(InvokeToStringWithCulture(_valueType, _value, _options));
        else if (_value is IDictionary dict) AddDictionary(dict);
        else if (_value is IEnumerable list) AddCollection(list);
        else AddComplexType();
    }

    private static bool TryFormatValue(object value, DumperOptions options, out string formattedValue) {
        formattedValue = null!;
        switch (value) {
            case char: formattedValue = $"'{value}'"; return true;
            case bool b: formattedValue = b ? "true" : "false"; return true;
            case decimal v: formattedValue = v.ToString(options.Culture); return true;
            case string: formattedValue = $"\"{value}\""; return true;
            case DateTime: formattedValue = $"{value:O}"; return true;
            case DateTimeOffset: formattedValue = $"{value:O}"; return true;
            case DateOnly: formattedValue = $"{value:O}"; return true;
            case TimeOnly: formattedValue = $"{value:O}"; return true;
            case TimeSpan: formattedValue = $"{value:c}"; return true;
            case Guid: formattedValue = $"{value:d}"; return true;
            case CultureInfo: formattedValue = $"{value}"; return true;
            case Type t: formattedValue = $"{t.FullName}"; return true;
            default: return false;
        }
    }

    private void AddCollection(IEnumerable items) {
        StartBlock("[");
        var count = 0;
        var enumerator = items.GetEnumerator();
        try {
            var hasNext = enumerator.MoveNext();
            while (hasNext) {
                if (_options.IndentOutput) AddIndentation();
                Execute(_level, _options.ShowListIndexes ? count : null, enumerator.Current,
                        _options,
                        _builder);
                hasNext = enumerator.MoveNext();
                if (!hasNext) break;
                count++;
                _builder.Append(',');
                if (_options.IndentOutput) AddNewLine();
            }
        }
        finally {
            (enumerator as IDisposable)?.Dispose();
        }
        if (_options.IndentOutput) AddNewLine();
        EndBlock("]");
    }

    private void AddDictionary(IDictionary dictionary) {
        StartBlock("[");
        var enumerator = dictionary.GetEnumerator();
        try {
            var hasNext = enumerator.MoveNext();
            while (hasNext) {
                if (_options.IndentOutput) AddIndentation();
                Execute(_level, enumerator.Key, enumerator.Value,
                        _options,
                        _builder);
                hasNext = enumerator.MoveNext();
                if (!hasNext) break;
                _builder.Append(',');
                if (_options.IndentOutput) AddNewLine();
            }
        }
        finally {
            (enumerator as IDisposable)?.Dispose();
        }
        if (_options.IndentOutput) AddNewLine();
        EndBlock("]");
    }

    private void AddComplexType() {
        StartBlock("{");
        var enumerator = GetMembers(_valueType).GetEnumerator();
        try {
            var hasNext = enumerator.MoveNext();
            while (hasNext) {
                if (_options.IndentOutput) AddIndentation();
                var member = (MemberInfo)enumerator.Current!;
                var value = member is FieldInfo field
                                ? field.GetValue(_value)
                                : ((PropertyInfo)member).GetValue(_value);
                Execute(_level, member.Name, value,
                        _options,
                        _builder);
                hasNext = enumerator.MoveNext();
                if (!hasNext) break;
                _builder.Append(',');
                if (_options.IndentOutput) AddNewLine();
            }
        }
        finally {
            (enumerator as IDisposable)?.Dispose();
        }
        if (_options.IndentOutput) AddNewLine();
        EndBlock("}");
    }

    private void StartBlock(string? prefix) {
        if (string.IsNullOrEmpty(prefix)) return;
        _builder.Append(prefix);
        if (_options.IndentOutput) AddNewLine();
        _level++;
    }

    private void EndBlock(string? suffix) {
        if (string.IsNullOrEmpty(suffix)) return;
        _level--;
        if (_options.IndentOutput) AddIndentation();
        _builder.Append(suffix);
    }

    private static MemberInfo[] GetMembers(IReflect type) {
        var members = type.GetMembers(BindingFlags.Public | BindingFlags.Instance);
        return members.Where(m => m.MemberType is MemberTypes.Field or MemberTypes.Property).ToArray();
    }

    private void AddIndentation() {
        if (_options.UseTabs) _builder.Append('\t', _level);
        else _builder.Append(' ', _level * _options.IndentSize);
    }

    private void AddNewLine() {
        if (_options.NewLineType == NewLineType.Windows) _builder.Append("\r\n");
        else _builder.Append('\n');
    }

    private static string InvokeToStringWithCulture(IReflect objectType, object value, DumperOptions options) {
        var methodInfo = objectType.GetMethod("ToString", BindingFlags.Instance | BindingFlags.Public, null, [typeof(CultureInfo)], null);
        return methodInfo is null
           ? value.ToString()!
           : methodInfo.Invoke(value, new object[] { options.Culture })!.ToString()!;
    }
}
