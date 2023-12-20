using System.Text.Json;

namespace DotNetToolbox;

internal sealed class DumpBuilder : IDisposable {
    private readonly DumpBuilderOptions _options;
    private readonly Stack<object> _ancestors;

    private readonly byte _level;
    private readonly Member _member;

    public bool SkipMember { get; private set; }
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

    private static readonly JsonSerializerOptions _indentedJson = new() {
        WriteIndented = true,
    };
    private static readonly JsonSerializerOptions _compactJson = new() {
        WriteIndented = false,
    };
    public static string Build(object? value, DumpBuilderOptions options) {
        if (value is null) return "null";
        if (options.Layout == Layout.Json) {
            return JsonSerializer.Serialize(value, options.Indented ? _indentedJson : _compactJson);
        }

        var member = new Member(MemberKind.Basic, null, value.GetType(), value);
        using var dumper = new DumpBuilder(0, member, options);
        dumper.AddType(member);
        return dumper.AddFormattedValue(member)
           ? dumper.Builder.ToString()
           : string.Empty;
    }

    private void AddType(Member member) {
        if (member.Type is null) return;
        Builder.Append('<');
        Builder.Append(member.Type.IsSubclassOf(typeof(Attribute))
                           ? nameof(Attribute)
                           : GetDescription(member.Type));
        Builder.Append('>');
        AddSpacer();
    }

    private bool AddFormattedValue(Member member) {
        SkipMember = false;
        if (TryUseCustomFormatter(member)) return true;
        if (!SkipMember && TryUseDefaultFormatter(member.Value)) return true;
        return !SkipMember && TryFormatComplexType(member.Value!);
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
            RuntimeTypeHandle => HideMember(),
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
            Assembly a when _level > 0 => GetDescription(a),
            Module m when _level > 0 => GetDescription(m),
            MemberInfo m when _level > 0 => GetDescription(m),
            Attribute m when _level > 0 => GetDescription(m),
            CustomAttributeData m when _level > 0 => GetDescription(m),
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
        => (_options.UseFullNames ? null : assembly.GetName().Name) ?? assembly.GetName().FullName;

    public string GetDescription(Module module)
        => _options.UseFullNames ? module.FullyQualifiedName : module.Name;

    public string? GetDescription(MemberInfo member) {
        var formattedValue = member switch {
            FieldInfo { IsPublic: false } or
            MethodBase { IsPublic: false } or
            PropertyInfo { GetMethod: null or { IsPublic: false } } or
            Type { IsPublic: false } => HideMember(),
            Type t => GetDescription(t),
            ConstructorInfo m => $"<{member.MemberType}> {m.Name}({string.Join(", ", m.GetParameters().Select(p => $"{GetDescription(p.ParameterType)} {p.Name}"))})",
            MethodInfo m => $"<{member.MemberType}> {GetDescription(m.ReturnType)} {m.Name}({string.Join(", ", m.GetParameters().Select(p => $"{GetDescription(p.ParameterType)} {p.Name}"))})",
            EventInfo e => $"<{member.MemberType}> {GetDescription(e.EventHandlerType!)} {e.Name}",
            PropertyInfo p => $"<{member.MemberType}> {GetDescription(p.PropertyType)} {p.Name}",
            FieldInfo f => $"<{member.MemberType}> {GetDescription(f.FieldType)} {f.Name}",
            _ => $"<{member.MemberType}> {member.Name}",
        };

        return formattedValue;
    }

    public string GetDescription(CustomAttributeData data)
        => GetDescription(data.AttributeType);

    public string GetDescription(Attribute attribute)
        => GetDescription(attribute.GetType());

    private string? HideMember() {
        SkipMember = true;
        return null;
    }

    private bool TryFormatComplexType(object value) {
        if (MaxLevelReached()) return false;

        StartBlock(value);
        AddMembers(value);
        EndBlock(value);
        return true;
    }

    private bool MaxLevelReached() {
        if (_options.MaxLevel == 0 || _level < _options.MaxLevel - 1) return false;
        Builder.Append("...");
        return true;

    }

    private void AddMembers(object value) {
        var counter = 0;
        var items = GetItems(value).Cast<object?>().Select((item, index) => (Value: item, Index: index)).ToArray();
        var lastIndex = items.Length > 0 ? items.Max(i => i.Index) : 0;
        foreach (var item in items) {
            var member = GetElementOrDefault(value, item.Value, counter);
            if (!TryAddValue(member)) continue;
            if (item.Index != lastIndex) Builder.Append(',');
            AddNewLine();
            counter++;
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

    private Member GetElementOrDefault(object? member, object? item, int count) {
        try {
            if (item is null) return default;
            if (HasCircularReference()) return default;
            if (member is IDictionary) {
                var itemType = item.GetType();
                var key = itemType.GetProperty("Key")!.GetValue(item);
                var value = itemType.GetProperty("Value")!.GetValue(item);
                return new(MemberKind.KeyValuePair, key, null, value);
            }

            if (member is IEnumerable) {
                return new(MemberKind.Element, GetIndex(), null, item);
            }

            var prop = (PropertyInfo)item;
            return new(MemberKind.Property, prop.Name, prop.PropertyType, prop.GetValue(member));

            bool HasCircularReference() => _ancestors.Any(a => ReferenceEquals(a, item));
            object? GetIndex() => _options.Layout == Layout.Typed ? count : null;
        }
        catch {
            return default;
        }
    }

    private void StartBlock(object value) {
        Builder.Append(value is IEnumerable ? '[' : '{');
        AddNewLine();
    }

    private bool TryAddValue(Member member) {
        if (member == default) return false;
        if (_level >= _options.MaxLevel) return false;
        if (_ancestors.Any(a => ReferenceEquals(a, member.Value))) return false;

        using var valueDumper = new DumpBuilder((byte)(_level + 1), member, _options, _ancestors);
        valueDumper.AddIndentation();
        valueDumper.AddFormattedName(member);
        valueDumper.AddType(member);
        var success = valueDumper.AddFormattedValue(member);
        if (valueDumper.SkipMember) return false;
        if (success) Builder.Append(valueDumper.Builder);
        return success;
    }

    private void AddFormattedName(Member member) {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (member.Kind) {
            case MemberKind.KeyValuePair:
                Builder.Append('[');
                var key = new Member(MemberKind.Basic, null, null, member.Name);
                AddFormattedValue(key);
                Builder.Append(']');
                AddSpacer();
                Builder.Append('=');
                break;
            case MemberKind.Element:
                Builder.Append($"{_member.Name}:");
                break;
            case MemberKind.Property:
                Builder.Append($"\"{_member.Name}\":");
                break;
        }

        AddSpacer();
    }

    private void AddSpacer() {
        if (!_options.Indented) return;
        Builder.Append(' ');
    }

    private void EndBlock(object value) {
        AddIndentation();
        Builder.Append(value is IEnumerable ? ']' : '}');
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
