namespace DotNetToolbox.AI.Jobs;

public class JobContext
    : Map, IJobContext {
    public JobContext(IEnumerable<KeyValuePair<string, object>>? source = null)
        : base(source) {
        World = [];
        UserProfile = new(0);
        Persona = new(0);
        Task = new(0);
        Assets = [];
        Tools = [];
        Memory = [];
    }

    public object Input {
        get => this[nameof(Input)]!;
        set => this[nameof(Input)] = value;
    }

    public object Output {
        get => this[nameof(Output)]!;
        set => this[nameof(Output)] = value;
    }

    public required Model Model {
        get => (Model)this[nameof(Model)]!;
        init => this[nameof(Model)] = value;
    }

    public required IAgent Agent {
        get => (IAgent)this[nameof(Agent)];
        init => this[nameof(Agent)] = value;
    }

    public UserProfile UserProfile {
        get => (UserProfile?)this[nameof(UserProfile)]!;
        init => this[nameof(UserProfile)] = value;
    }

    public World World {
        get => (World)this[nameof(World)];
        init => this[nameof(World)] = value;
    }

    public Map<Asset> Assets {
        get => (Map<Asset>)this[nameof(Assets)];
        init => this[nameof(Assets)] = value;
    }

    public Map<Tool> Tools {
        get => (Map<Tool>)this[nameof(Tools)];
        init => this[nameof(Tools)] = value;
    }

    public Persona Persona {
        get => (Persona)this[nameof(Persona)]!;
        init => this[nameof(Persona)] = value;
    }

    public Map<string> Memory {
        get => (Map<string>)this[nameof(Memory)];
        init => this[nameof(Memory)] = value;
    }

    public Task Task {
        get => (Task)this[nameof(Task)];
        init => this[nameof(Task)] = value;
    }

    public IMap OutputAsMap => Output as IMap ?? throw new InvalidCastException("Output is not a Map.");
    public IReadOnlyList<object> OutputAsList => Output as IReadOnlyList<object> ?? throw new InvalidCastException("Output is not a List.");
    public IReadOnlyList<IReadOnlyList<object>> OutputAsTable => Output as IReadOnlyList<IReadOnlyList<object>> ?? throw new InvalidCastException("Output is not a Table.");
}
