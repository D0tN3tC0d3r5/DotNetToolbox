namespace DotNetToolbox.ValidationBuilder.Commands;

public sealed class ValidationCommandFactory {
    private readonly Type _subjectType;
    private readonly string _source;

    private ValidationCommandFactory(Type subjectType, string source) {
        _subjectType = subjectType;
        _source = source;
    }

    public static ValidationCommandFactory For(Type subjectType, string source)
        => new(subjectType, source);

    public IValidationCommand Create(string command, params object?[] arguments) {
        #pragma warning disable IDE0046 // Convert to conditional expression
        // ReSharper disable once ConvertIfStatementToSwitchStatement - Better readability
        if (command == "IsNull") return new IsNullCommand(_source);
        if (command == "IsEqualTo") return new IsEqualToCommand(arguments[0]!, _source);
        if (_subjectType == typeof(int)) return CreateNumberCommand<int>(command, arguments);
        if (_subjectType == typeof(decimal)) return CreateNumberCommand<decimal>(command, arguments);
        if (_subjectType == typeof(DateTime)) return CreateDateTimeCommand(command, arguments);
        if (_subjectType == typeof(int?)) return CreateNumberCommand<int>(command, arguments);
        if (_subjectType == typeof(decimal?)) return CreateNumberCommand<decimal>(command, arguments);
        if (_subjectType == typeof(DateTime?)) return CreateDateTimeCommand(command, arguments);
        if (_subjectType == typeof(string)) return CreateStringCommand(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(ICollection<int>))) return CreateCollectionCommand<int>(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(ICollection<decimal>))) return CreateCollectionCommand<decimal>(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(ICollection<int?>))) return CreateCollectionCommand<int>(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(ICollection<decimal?>))) return CreateCollectionCommand<decimal>(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(ICollection<string?>))) return CreateCollectionCommand<string>(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(IDictionary<string, int>))) return CreateDictionaryCommand<string, int>(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(IDictionary<string, decimal>))) return CreateDictionaryCommand<string, decimal>(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(IDictionary<string, int?>))) return CreateDictionaryCommand<string, int>(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(IDictionary<string, decimal?>))) return CreateDictionaryCommand<string, decimal>(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(IDictionary<string, string>))) return CreateDictionaryCommand<string, string>(command, arguments);
        if (_subjectType.IsAssignableTo(typeof(IValidatable))) return CreateValidatableCommand(command);
        throw new InvalidOperationException($"Unsupported command '{command}' for type '{_subjectType.Name}'.");
        #pragma warning restore IDE0046 // Convert to conditional expression
    }

    private IsValidCommand CreateValidatableCommand(string command)
        => command switch {
            "IsValid" => new(_source),
            _ => throw new InvalidOperationException($"Unsupported command '{command}' for {_subjectType.Name}."),
           };

    private IValidationCommand CreateNumberCommand<TValue>(string command, IReadOnlyList<object?> arguments)
        where TValue : struct, IComparable<TValue> {
        return command switch {
            "IsLessThan" => new IsLessThanCommand<TValue>(GetLimitValue(), _source),
            "IsGreaterThan" => new IsGreaterThanCommand<TValue>(GetLimitValue(), _source),
            _ => throw new InvalidOperationException($"Unsupported command '{command}' for type '{_subjectType.Name}'."),
               };

        TValue GetLimitValue() => GetArgumentValue<TValue>(command, arguments, 0);
    }

    private IValidationCommand CreateDateTimeCommand(string command, IReadOnlyList<object?> arguments) {
        return command switch {
            "IsBefore" => new IsBeforeCommand(GetLimitValue(), _source),
            "IsAfter" => new IsAfterCommand(GetLimitValue(), _source),
            _ => throw new InvalidOperationException($"Unsupported command '{command}' for type '{_subjectType.Name}'."),
               };

        DateTime GetLimitValue() => GetArgumentValue<DateTime>(command, arguments, 0);
    }

    private IValidationCommand CreateStringCommand(string command, IReadOnlyList<object?> arguments) {
        return command switch {
            "IsEmpty" => new IsEmptyCommand(_source),
            "IsEmptyOrWhiteSpace" => new IsEmptyOrWhiteSpaceCommand(_source),
            "LengthIsAtLeast" => new LengthIsAtLeastCommand(GetLengthValue(), _source),
            "LengthIsAtMost" => new LengthIsAtMostCommand(GetLengthValue(), _source),
            "LengthIs" => new LengthIsCommand(GetLengthValue(), _source),
            "Contains" => new ContainsCommand(GetCandidateValue(), _source),
            "IsIn" => new IsInCommand<string>(GetListValue(), _source),
            "IsEmail" => new IsEmailCommand(_source),
            "IsValidPassword" => new IsValidPasswordCommand(GetPolicy(), _source),
            _ => throw new InvalidOperationException($"Unsupported command '{command}' for type '{_subjectType.Name}'."),
               };

        int GetLengthValue() => GetArgumentValue<int>(command, arguments, 0);
        string GetCandidateValue() => GetArgumentValue<string>(command, arguments, 0);
        string?[] GetListValue() => [.. GetArgumentValues<string>(command, arguments)];
        IValidatable GetPolicy() => GetArgumentValue<IValidatable>(command, arguments, 0);
    }

    private IValidationCommand CreateCollectionCommand<TItem>(string command, IReadOnlyList<object?> arguments) {
        return command switch {
            "IsEmpty" => new IsEmptyCommand<TItem>(_source),
            "Contains" => new ContainsCommand<TItem>(GetItemValue(), _source),
            "HasAtLeast" => new HasAtLeastCommand<TItem>(GetCountValue(), _source),
            "HasAtMost" => new HasAtMostCommand<TItem>(GetCountValue(), _source),
            "Has" => new HasCommand<TItem>(GetCountValue(), _source),
            _ => throw new InvalidOperationException($"Unsupported command '{command}' for type '{_subjectType.Name}'."),
               };

        int GetCountValue() => GetArgumentValue<int>(command, arguments, 0);
        TItem? GetItemValue() => GetArgumentValueOrDefault<TItem>(command, arguments, 0);
    }

    private IValidationCommand CreateDictionaryCommand<TKey, TValue>(string command, IReadOnlyList<object?> arguments)
        where TKey : notnull {
        return command switch {
            "IsEmpty" => new IsEmptyCommand<KeyValuePair<TKey, TValue?>>(_source),
            "HasAtLeast" => new HasAtLeastCommand<KeyValuePair<TKey, TValue?>>(GetCountValue(), _source),
            "HasAtMost" => new HasAtMostCommand<KeyValuePair<TKey, TValue?>>(GetCountValue(), _source),
            "Has" => new HasCommand<KeyValuePair<TKey, TValue?>>(GetCountValue(), _source),
            "ContainsKey" => new ContainsKeyCommand<TKey, TValue?>(GetKeyValue(), _source),
            "ContainsValue" => new ContainsValueCommand<TKey, TValue?>(GetValueValue(), _source),
            _ => throw new InvalidOperationException($"Unsupported command '{command}' for type '{_subjectType.Name}'."),
               };

        int GetCountValue() => GetArgumentValueOrDefault<int>(command, arguments, 0);
        TKey GetKeyValue() => GetArgumentValue<TKey>(command, arguments, 0);
        TValue? GetValueValue() => GetArgumentValueOrDefault<TValue>(command, arguments, 0);
    }

    [return: NotNull]
    private static TArgument GetArgumentValue<TArgument>(string methodName, IReadOnlyList<object?> arguments, uint argumentIndex, [CallerArgumentExpression(nameof(arguments))] string? paramName = null)
        => argumentIndex >= arguments.Count
            ? throw new ArgumentException($"Invalid number of arguments for '{methodName}'. Missing argument {argumentIndex}.", paramName)
            : arguments[(int)argumentIndex] is TArgument value
                    ? value
                    : throw new ArgumentException($"Invalid type of {paramName}[{argumentIndex}] of '{methodName}'. Expected: {typeof(TArgument).Name}. Found: {arguments[(int)argumentIndex]!.GetType().Name}.", $"{paramName}[{argumentIndex}]");

    private static TArgument? GetArgumentValueOrDefault<TArgument>(string methodName, IReadOnlyList<object?> arguments, uint argumentIndex, [CallerArgumentExpression(nameof(arguments))] string? paramName = null)
        => argumentIndex >= arguments.Count
            ? throw new ArgumentException($"Invalid number of arguments for '{methodName}'. Missing argument {argumentIndex}.", paramName)
            : arguments[(int)argumentIndex] switch {
                null => default,
                TArgument value => value,
                _ => throw new ArgumentException($"Invalid type of {paramName}[{argumentIndex}] of '{methodName}'. Expected: {typeof(TArgument).Name}. Found: {arguments[(int)argumentIndex]!.GetType().Name}.", $"{paramName}[{argumentIndex}]"),
            };

    private static TArgument?[] GetArgumentValues<TArgument>(string methodName, IReadOnlyList<object?> arguments, [CallerArgumentExpression(nameof(arguments))] string? paramName = null) {
        var list = IsNotNullOrEmpty(arguments, paramName);
        for (var index = 0; index < list.Count; index++) GetArgumentValueOrDefault<TArgument>(methodName, arguments, (uint)index, paramName);

        return list.Select(i => i is null ? default : (TArgument)i).ToArray();
    }
}
