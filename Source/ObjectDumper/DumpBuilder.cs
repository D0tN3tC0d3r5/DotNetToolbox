namespace DotNetToolbox;

internal record DumpBuilder {
    private readonly Member _member;
    private readonly DumpBuilderOptions _options;
    private readonly HashSet<object> _ancestors;

    private readonly byte _level;

    public bool Success { get; private set; }
    public bool SkipMember { get; private set; }
    public StringBuilder Builder { get; }

    private DumpBuilder(byte level, Member member, DumpBuilderOptions options, HashSet<object>? ancestors = null) {
        _member = member;
        _options = options;
        _level = level;
        Builder = new();
        _ancestors = ancestors ?? [];
        if (_member.Value is not null) _ancestors.Add(_member.Value);
    }

    public static string Build(object? value, DumpBuilderOptions options) {
        var member = new Member(null, value?.GetType(), value);
        var dumper = new DumpBuilder(0, member, options);
        dumper.AddType();
        dumper.AddFormattedValue(value);
        return dumper.Build();
    }

    public string Build() => Builder.ToString();

    private void AddType() {
        if (_options.Layout == Layout.Json || _member.Type is null) return;
        Builder.Append('<');
        Builder.Append(_member.Type.IsSubclassOf(typeof(Attribute))
                           ? nameof(Attribute)
                           : GetDescription(_member.Type));
        Builder.Append('>');
        AddSpacer();
    }

    private void AddFormattedValue(object? value) {
        Success = false;
        SkipMember = false;
        TryUseCustomFormatter(value);
        if (Success || SkipMember) return;
        TryUseDefaultFormatter(value);
        if (Success || SkipMember) return;
        TryFormatComplexType(value!);
    }

    private void TryUseCustomFormatter(object? value) {
        var formattedValue = value is not null
                          && _options.CustomFormatters.TryGetValue(value.GetType(), out var formatter)
                                 ? formatter(value)
                                 : null;
        if (formattedValue is null) return;

        Builder.Append(formattedValue);
        Success = true;
    }

    private void TryUseDefaultFormatter([NotNullWhen(false)] object? value) {
        var formattedValue = value switch {
            null => "null",
            RuntimeTypeHandle => HideMember(),
            bool => $"{value}".ToLower(),
            nint => $"{value}",
            nuint => $"{value}",
            char => $"'{value}'",
            string => $"\"{value}\"",
            Guid v => $"{v:d}",
            TimeSpan v => $"{v:g}",
            IConvertible v => v.ToString(CultureInfo.CurrentCulture),
            DateTimeOffset v => v.ToString(CultureInfo.CurrentCulture),
            DateOnly v => v.ToString(CultureInfo.CurrentCulture),
            TimeOnly v => v.ToString(CultureInfo.CurrentCulture),
            Assembly a when _level > 0 => GetDescription(a),
            Module m when _level > 0 => GetDescription(m),
            MemberInfo m when _level > 0 => GetDescription(m),
            Attribute m when _level > 0 => GetDescription(m),
            CustomAttributeData m when _level > 0 => GetDescription(m),
            _ => null,
        };

        if (formattedValue is null) return;
        Builder.Append(formattedValue);
        Success = true;
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
        Success = false;
        var formattedValue = member switch {
                 FieldInfo { IsPublic: false } or
                 MethodBase { IsPublic: false } or
                 PropertyInfo { GetMethod: null or { IsPublic: false } } or
                 Type { IsPublic: false } => HideMember(),
            Type t => GetDescription(t),
            ConstructorInfo m => $"<{member.MemberType}> {m.Name}({string.Join(", ", m.GetParameters().Select(p => $"{GetDescription(p.ParameterType)} {p.Name}"))})",
            MethodInfo m => $"<{member.MemberType}> {GetDescription(m.ReturnType)} {GetDescription(m)}({string.Join(", ", m.GetParameters().Select(p => $"{GetDescription(p.ParameterType)} {p.Name}"))})",
            EventInfo e => $"<{member.MemberType}> {GetDescription(e.EventHandlerType!)} {e.Name}",
            PropertyInfo p => $"<{member.MemberType}> {GetDescription(p.PropertyType)} {p.Name}",
            FieldInfo f => $"<{member.MemberType}> {GetDescription(f.FieldType)} {f.Name}",
            _ => $"<{member.MemberType}> {member.Name}",
        };

        Success = formattedValue is not null;
        return formattedValue;
    }

    public string GetDescription(MethodInfo method) {
        var methodName = method.Name;
        var genericStart = methodName.IndexOf('`');
        if (genericStart < 0) return methodName;
        var genericArguments = method.GetGenericArguments();
        var genericArgumentsNames = string.Join(", ", genericArguments.Select(GetDescription));
        return $"{methodName[..genericStart]}<{genericArgumentsNames}>";

    }

    public string GetDescription(CustomAttributeData data)
        => GetDescription(data.AttributeType);

    public string GetDescription(Attribute attribute)
        => GetDescription(attribute.GetType());

    private string? HideMember() {
        SkipMember = true;
        return null;
    }

    private void TryFormatComplexType(object value) {
        if (MaxLevelReached()) return;

        StartBlock();
        AddMembers(value);
        EndBlock();
        Success = true;
    }

    private bool MaxLevelReached() {
        if (_options.MaxLevel == 0 || _level < _options.MaxLevel - 1) return false;
        Builder.Append("...");
        return true;

    }

    private void AddMembers(object value) {
        var counter = 0;
        // ReSharper disable once NotDisposedResource = not disposable
        var enumerator = value.GetEnumerator()!;
        var hasNext = enumerator.MoveNext();
        while (hasNext) {
            hasNext = AddMember(ref counter, value, enumerator);
        }
    }

    private bool AddMember(ref int counter, object value, IEnumerator enumerator) {
        var memberFound = value is IEnumerable
            ? TryGetElement(enumerator, counter, out var member)
            : TryGetMember(enumerator, out member);
        var memberAdded = memberFound && TryAddValue(member);
        var hasNext = enumerator.MoveNext();
        if (!memberAdded) return hasNext;
        if (hasNext) Builder.Append(',');
        AddNewLine();
        counter++;
        return hasNext;
    }

    private bool TryGetElement(IEnumerator enumerator, int count, out Member member) {
        try {
            member = GetElement(enumerator, count) ?? Member.Default;
            return member != Member.Default;
        }
        catch (Exception) {
            member = Member.Default;
            return false;
        }

    }

    private Member? GetElement(IEnumerator enumerator, int count) {
        var element = enumerator.Current;
        return HasCircularReference()
                   ? null
                   : enumerator is IDictionaryEnumerator keyValuePair
                       ? new(keyValuePair.Key, null, keyValuePair.Value)
                       : new(GetIndex(), null, element);

        bool HasCircularReference() => element is not null && _ancestors.Any(a => ReferenceEquals(a, element));
        object? GetIndex() => _options.Layout == Layout.Typed ? count : null;
    }

    private bool TryGetMember(IEnumerator enumerator, out Member member) {
        member = Member.Default;
        var prop = (PropertyInfo)enumerator.Current!;
        try {
            member = new(prop.Name, prop.PropertyType, prop.GetValue(_member.Value));
            return true;
        }
        catch {
            return false;
        }
    }

    private void StartBlock() {
        Builder.Append(_member.IsEnumerable ? '[' : '{');
        AddNewLine();
    }

    private bool TryAddValue(Member member) {
        if (_level >= _options.MaxLevel) return false;
        if (member.Value is not null && _ancestors.Any(a => ReferenceEquals(a, member.Value))) return false;

        var valueDumper = new DumpBuilder((byte)(_level + 1), member, _options, _ancestors);
        valueDumper.AddIndentation();
        valueDumper.AddFormattedName();
        valueDumper.AddType();
        valueDumper.AddFormattedValue(member.Value);
        if (valueDumper.SkipMember) return false;
        if (valueDumper.Success) Builder.Append(valueDumper.Builder);
        return valueDumper.Success;
    }

    private void AddFormattedName() {
        if (_member.Name is null) return;
        AddFormattedValue(_member.Name);
        Builder.Append(':');
        AddSpacer();
    }

    private void AddSpacer() {
        if (!_options.Indented) return;
        Builder.Append(' ');
    }

    private void EndBlock() {
        AddIndentation();
        Builder.Append(_member.IsEnumerable ? ']' : '}');
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
