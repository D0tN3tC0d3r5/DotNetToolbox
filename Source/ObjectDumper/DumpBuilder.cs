namespace DotNetToolbox;

internal sealed class DumpBuilder : IDisposable {
    private readonly DumpBuilderOptions _options;
    private readonly Stack<object> _ancestors;

    private readonly byte _level;
    private readonly Member _member;

    public bool MemberIsHidden { get; private set; }
    public StringBuilder Builder { get; }

    private DumpBuilder(byte level, Member member, DumpBuilderOptions options, Stack<object>? ancestors = null) {
        _options = options;
        _level = level;
        _member = member;
        Builder = new();
        _ancestors = ancestors ?? [];
        if (_member.Value is not null) _ancestors.Push(_member.Value);
    }


    void IDisposable.Dispose() {
        if (_member.Value is not null) _ancestors.Pop();
    }

    public static string Build(object? value, DumpBuilderOptions options) {
        if (value is null) return "null";
        var member = new Member(MemberKind.Object, null, value.GetType(), value);
        using var dumper = new DumpBuilder(0, member, options);
        dumper.AddType(member);
        dumper.AddFormattedValue(member);
        return dumper.Builder.ToString();
    }

    private void AddType(Member member) {
        if (member.Type is null) return;
        AddSymbol('<');
        Builder.Append(member.Type.IsSubclassOf(typeof(Attribute))
                           ? nameof(Attribute)
                           : GetDescription(member.Type));
        AddSymbol('>');
        AddSpacer();
    }

    private bool AddFormattedValue(Member member) {
        if (TryAddKeyWord(member)) return true;
        if (TryUseCustomFormatter(member)) return true;
        if (TryUseDefaultFormatter(member.Value)) return true;
        if (TryFormatSpecialType(member.Value)) return true;
        if (MemberIsHidden) return false;
        AddFormattedComplexType(member.Value!);
        return true;
    }

    private bool TryAddKeyWord(Member member) {
        if (member.Kind != MemberKind.KeyWord) return false;
        AddSymbol('#');
        Builder.Append(member.Value);
        AddSymbol('#');
        return true;
    }

    private bool TryUseCustomFormatter(Member member) {
        if (member.Type is null) return false;
        var formattedValue = _options
                            .CustomFormatters
                            .TryGetValue(member.Type, out var formatter)
                                 ? formatter(member.Value)
                                 : null;
        if (formattedValue is null) return false;

        Builder.Append(formattedValue);
        return true;
    }

    private bool TryUseDefaultFormatter([NotNullWhen(false)] object? value) {
        var formattedValue = value switch {
            null => "null",
            bool => $"{value}".ToLower(),
            nint => $"{value}",
            nuint => $"{value}",
            string => $"\"{value}\"",
            char => $"'{value}'",
            Guid => $"{value}",
            TimeSpan => $"{value}",
            DateTime v => v.ToString(CultureInfo.CurrentCulture),
            DateTimeOffset v => v.ToString(CultureInfo.CurrentCulture),
            DateOnly v => v.ToString(CultureInfo.CurrentCulture),
            TimeOnly v => v.ToString(CultureInfo.CurrentCulture),
            IConvertible v => v.ToString(CultureInfo.CurrentCulture),
            _ => null,
        };

        if (formattedValue is null) return false;
        Builder.Append(formattedValue);
        return true;
    }

    public string GetDescription(Type type) {
        var typeName = (_options.UseFullNames ? type.FullName : null) ?? type.Name;
        var genericStart = typeName.IndexOf('`');
        if (genericStart < 0) return typeName;
        var genericArguments = type.GetGenericArguments();
        var genericArgumentsNames = string.Join(", ", genericArguments.Select(GetDescription));
        var declaringType = type.DeclaringType is null ? string.Empty : $"{GetDescription(type.DeclaringType!)}+";
        return $"{declaringType}{typeName[..typeName.IndexOf('`')]}<{genericArgumentsNames}>";
    }

    public string GetDescription(Assembly assembly)
        => _options.UseFullNames
               ? assembly.GetName().FullName
               : $"{assembly.GetName().Name} v{assembly.GetName().Version}";

    #pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
    public string? GetDescription(MemberInfo member)
        => member switch {
               FieldInfo { IsPublic: false }
                 or MethodBase { IsPublic: false }
                 or PropertyInfo { GetMethod: null or { IsPublic: false } }
                 or Type { IsPublic: false } => HideMember(),
               ConstructorInfo m => $"<{member.MemberType}> {m.Name}({string.Join(", ", m.GetParameters().Select(p => $"{GetDescription(p.ParameterType)} {p.Name}"))})",
               FieldInfo f => $"<{member.MemberType}> {GetDescription(f.FieldType)} {f.Name}",
               MethodInfo m => $"<{member.MemberType}> {GetDescription(m.ReturnType)} {m.Name}({string.Join(", ", m.GetParameters().Select(p => $"{GetDescription(p.ParameterType)} {p.Name}"))})",
               EventInfo e => $"<{member.MemberType}> {GetDescription(e.EventHandlerType!)} {e.Name}",
               PropertyInfo p => $"<{member.MemberType}> {GetDescription(p.PropertyType)} {p.Name}",
               Type t => GetDescription(t),
           };
    #pragma warning restore CS8509


    public string GetDescription(CustomAttributeData data)
        => GetDescription(data.AttributeType);

    public string GetDescription(Attribute attribute)
        => GetDescription(attribute.GetType());

    private string? HideMember() {
        MemberIsHidden = true;
        return null;
    }

    private bool TryFormatSpecialType(object value) {
        var formattedValue = value switch {
            RuntimeTypeHandle when _level > 0 => HideMember(),
            Module when _level > 0 => HideMember(),
            Assembly a when _level > 0 => GetDescription(a),
            MemberInfo m when _level > 0 => GetDescription(m),
            Attribute m when _level > 0 => GetDescription(m),
            CustomAttributeData m when _level > 0 => GetDescription(m),
            _ => null,
        };

        if (formattedValue is null) return false;
        Builder.Append(formattedValue);
        return true;
    }

    private void AddFormattedComplexType(object value) {
        if (MaxDepthReached()) return;
        StartBlock(value);
        AddMembers(value);
        EndBlock(value);
    }

    private bool MaxDepthReached() {
        if (_level < _options.MaxDepth) return false;
        Builder.Append("...");
        return true;
    }

    private void AddMembers(object value) {
        var items = GetItems(value).Cast<object?>().Select((item, index) => (Value: item, Index: index)).ToArray();
        var lastIndex = items.Length > 0 ? items.Max(i => i.Index) : 0;
        foreach (var item in items) {
            var member = GetElementOrDefault(value, item.Value);
            if (!TryAddValue(member)) continue;
            if (item.Index != lastIndex) AddSymbol(',');
            AddNewLine();
        }
        RemoveExtraComma();
    }

    private static readonly int _commaAndNewLineLength = Environment.NewLine.Length + 1;
    private void RemoveExtraComma() {
        if (!_options.Indented) return;
        if (Builder[^_commaAndNewLineLength] != ',') return;
        Builder.Remove(Builder.Length - _commaAndNewLineLength, _commaAndNewLineLength);
        AddNewLine();
    }

    private static IEnumerable GetItems(object value)
        => value switch {
            IEnumerable list => list,
            _ => GetMembers(value.GetType()).AsEnumerable(),
        };

    private static readonly BindingFlags _allPublic = BindingFlags.Public | BindingFlags.Instance;
    private static PropertyInfo[] GetMembers(IReflect type)
        => type.GetProperties(_allPublic).ToArray();

    private Member? GetElementOrDefault(object? member, object? item) {
        try {
            if (item is null) return default(Member);
            if (HasCircularReference()) return new(MemberKind.KeyWord, null, null, "CircularReference");
            if (member is IDictionary) {
                var itemType = item.GetType();
                var key = itemType.GetProperty("Key")!.GetValue(item);
                var value = itemType.GetProperty("Value")!.GetValue(item);
                return new(MemberKind.KeyValuePair, key, null, value);
            }

            if (member is IEnumerable)
                return new(MemberKind.Element, null, null, item);

            var prop = (PropertyInfo)item;
            return new(MemberKind.Property, prop.Name, prop.PropertyType, prop.GetValue(member));

            bool HasCircularReference() => _ancestors.Any(a => ReferenceEquals(a, item));
        }
        catch {
            return null;
        }
    }

    private void StartBlock(object value) {
        AddSymbol(value is IEnumerable ? '[' : '{');
        AddNewLine();
    }

    private bool TryAddValue(Member? member) {
        if (member is null) return false;
        if (_ancestors.Any(a => ReferenceEquals(a, member.Value.Value))) return false;

        using var dumper = new DumpBuilder((byte)(_level + 1), member.Value, _options, _ancestors);
        dumper.AddIndentation();
        dumper.AddFormattedName(member.Value);
        dumper.AddType(member.Value);
        var success = dumper.AddFormattedValue(member.Value);
        if (dumper.MemberIsHidden) return false;
        if (success) Builder.Append(dumper.Builder);
        return success;
    }

    private void AddFormattedName(Member member) {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (member.Kind) {
            case MemberKind.KeyValuePair:
                AddSymbol('[');
                var key = new Member(MemberKind.Object, null, null, member.Name);
                AddFormattedValue(key);
                AddSymbol(']');
                AddSpacer();
                AddSymbol('=');
                AddSpacer();
                break;
            case MemberKind.Property:
                Builder.Append($"\"{_member.Name}\"");
                AddSymbol(':');
                AddSpacer();
                break;
        }
    }

    private void AddSpacer() {
        if (!_options.Indented) return;
        AddSymbol(' ');
    }

    private void AddSymbol(char symbol)
        => Builder.Append(symbol);

    private void EndBlock(object value) {
        AddIndentation();
        AddSymbol(value is IEnumerable ? ']' : '}');
    }

    private void AddIndentation() {
        if (!_options.Indented) return;
        if (_options.UseTabs) Builder.Append('\t', _level);
        else Builder.Append(' ', _level * _options.IndentSize);
    }

    private void AddNewLine() {
        if (!_options.Indented) return;
        Builder.Append(Environment.NewLine);
    }
}
