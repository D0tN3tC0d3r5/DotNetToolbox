namespace DotNetToolbox.AI.Agents;

public class Persona
    : Map, IValidatable {
    public Persona(string? name = null, string? description = null) {
        Name = name ?? "Agent";
        Description = description ?? "You are a helpful ASSISTANT.";
        Cognition = [];
        Disposition = [];
        Interaction = [];
        Attitude = [];
    }

    public string Name {
        get => (string)this[nameof(Name)];
        init => this[nameof(Name)] = value;
    }

    public string Description {
        get => (string)this[nameof(Description)];
        init => this[nameof(Description)] = value;
    }

    public List<string> Cognition {
        get => (List<string>)this[nameof(Cognition)];
        init => this[nameof(Cognition)] = value;
    }

    public List<string> Disposition {
        get => (List<string>)this[nameof(Disposition)];
        init => this[nameof(Disposition)] = value;
    }

    public List<string> Interaction {
        get => (List<string>)this[nameof(Interaction)];
        init => this[nameof(Interaction)] = value;
    }

    public List<string> Attitude {
        get => (List<string>)this[nameof(Attitude)];
        init => this[nameof(Attitude)] = value;
    }

    public Result Validate(IDictionary<string, object?>? context = null) => Result.Success();
}
