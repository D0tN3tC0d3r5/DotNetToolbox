namespace DotNetToolbox.AI.Jobs;

public class JobContext
    : Context {
    public JobContext(IEnumerable<KeyValuePair<string, object>>? source = null)
        : base(source) {
        Memory = [];
        Assets = [];
        Tools = [];
    }

    public Map Memory {
        get => (Map)this[nameof(Memory)];
        init => this[nameof(Memory)] = value;
    }

    public Map<Asset> Assets {
        get => (Map<Asset>)this[nameof(Assets)];
        init => this[nameof(Assets)] = value;
    }

    public Map<Tool> Tools {
        get => (Map<Tool>)this[nameof(Tools)];
        init => this[nameof(Tools)] = value;
    }

    public required World World {
        get => (World)this[nameof(World)];
        init => this[nameof(World)] = value;
    }

    public required UserProfile? User {
        get => (UserProfile?)this[nameof(User)]!;
        init => this[nameof(User)] = value;
    }

    public required IAgent Agent {
        get => (IAgent)this[nameof(Agent)];
        init => this[nameof(Agent)] = value;
    }
}
