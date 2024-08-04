namespace DotNetToolbox.AI.Tools;

public class Tool
    : Context, IValidatable {
    public Tool(string id,
                string name,
                string returnType,
                List<ToolArgument>? parameters = null,
                string? description = null) {
        this[nameof(Id)] = id;
        this[nameof(Name)] = name;
        this[nameof(ReturnType)] = returnType;
        this[nameof(Description)] = description;
        this[nameof(Arguments)] = parameters ?? [];
    }

    public required string Id {
        get => (string)this[nameof(Id)]!;
        init => this[nameof(Id)] = value;
    }

    public required string Name {
        get => (string)this[nameof(Name)]!;
        init => this[nameof(Name)] = value;
    }

    public required string ReturnType {
        get => (string)this[nameof(ReturnType)]!;
        init => this[nameof(ReturnType)] = value;
    }

    public string? Description {
        get => (string?)this[nameof(Description)];
        init => this[nameof(Description)] = value;
    }

    public List<ToolArgument> Arguments => (List<ToolArgument>)this[nameof(Arguments)]!;

    public string Signature => $"{Name}({string.Join(",", Arguments.Select(p => p.Signature))}) -> {ReturnType}";

    public Result Validate(IDictionary<string, object?>? context = null)
        => Result.Success();
}
