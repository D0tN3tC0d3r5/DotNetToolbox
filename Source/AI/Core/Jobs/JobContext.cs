namespace DotNetToolbox.AI.Jobs;

public class JobContext
    : Context {
    public JobContext(IEnumerable<KeyValuePair<string, object>>? source = null)
        : base(source) {
        World = [];
        User = new UserProfile(0);
        Persona = new Persona(0);
        Task = new Task(0);
        Assets = [];
        Tools = [];
        Memory = [];
    }

    public required Model Model {
        get => (Model)this[nameof(Model)]!;
        init => this[nameof(Model)] = value;
    }

    public required IHttpConnection Connection {
        get => (IHttpConnection)this[nameof(Connection)];
        init => this[nameof(Connection)] = value;
    }

    public UserProfile User {
        get => (UserProfile?)this[nameof(User)]!;
        init => this[nameof(User)] = value;
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

    public Map Memory {
        get => (Map)this[nameof(Memory)];
        init => this[nameof(Memory)] = value;
    }

    public Task Task {
        get => (Task)this[nameof(Task)];
        init => this[nameof(Task)] = value;
    }
}
