namespace DotNetToolbox.AI.Tools;

public class ToolArgument
    : Context, IValidatable {
    public ToolArgument(string name,
                        ToolArgumentType type,
                        string[]? options = null,
                        bool isRequired = false,
                        string? description = null) {
        this[nameof(Name)] = name;
        this[nameof(Type)] = type;
        this[nameof(Description)] = description;
        this[nameof(Options)] = options ?? [];
        this[nameof(IsRequired)] = isRequired;
    }

    public required string Name {
        get => (string)this[nameof(Name)]!;
        init => this[nameof(Name)] = value;
    }

    public required ToolArgumentType Type {
        get => (ToolArgumentType)this[nameof(Type)]!;
        init => this[nameof(Type)] = value;
    }

    public string? Description {
        get => (string?)this[nameof(Description)];
        init => this[nameof(Description)] = value;
    }

    public string[] Options => (string[])this[nameof(Options)]!;

    public bool IsRequired {
        get => (bool)this[nameof(IsRequired)]!;
        init => this[nameof(IsRequired)] = value;
    }

    public string Signature => IsRequired ? $"{Name}: {Type}" : $"[{Name}: {Type}]";

    public override string ToString()
        => $"{Signature}{(string.IsNullOrWhiteSpace(Description) ? string.Empty : $" /* {Description} */")}";

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}

