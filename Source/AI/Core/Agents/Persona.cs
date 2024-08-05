namespace DotNetToolbox.AI.Agents;

public class Persona
    : Context, IValidatable  {
    public Persona(string? name = null, string? description = null)  {
        this[nameof(Name)] = name ?? "Agent";
        this[nameof(Description)] = description ?? "You are a helpful ASSISTANT.";
        this[nameof(Cognition)] = new List<string>();
        this[nameof(Disposition)] = new List<string>();
        this[nameof(Interaction)] = new List<string>();
        this[nameof(Attitude)] = new List<string>();
    }

    public string Name {
        get => (string)this[nameof(Name)]!;
        init => this[nameof(Name)] = value;
    }

    public string Description {
        get => (string)this[nameof(Description)]!;
        init => this[nameof(Description)] = value;
    }

    public List<string> Cognition => (List<string>)this[nameof(Cognition)]!;
    public List<string> Disposition => (List<string>)this[nameof(Disposition)]!;
    public List<string> Interaction => (List<string>)this[nameof(Interaction)]!;
    public List<string> Attitude => (List<string>)this[nameof(Attitude)]!;

    public Result Validate(IDictionary<string, object?>? context = null) => Result.Success();
}
