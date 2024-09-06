namespace DotNetToolbox.AI.Jobs;

public class JobContext
    : Context, IJobContext {
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

    public required Model Model {
        get => (Model)this[nameof(Model)]!;
        init => this[nameof(Model)] = value;
    }

    public required IHttpConnection Connection {
        get => (IHttpConnection)this[nameof(Connection)];
        init => this[nameof(Connection)] = value;
    }

    public UserProfile UserProfile {
        get => (UserProfile?)this[nameof(UserProfile)]!;
        init => this[nameof(UserProfile)] = value;
    }

    public World World {
        get => (World)this[nameof(World)];
        init => this[nameof(World)] = value;
    }

    public Context<Asset> Assets {
        get => (Context<Asset>)this[nameof(Assets)];
        init => this[nameof(Assets)] = value;
    }

    public Context<Tool> Tools {
        get => (Context<Tool>)this[nameof(Tools)];
        init => this[nameof(Tools)] = value;
    }

    public Persona Persona {
        get => (Persona)this[nameof(Persona)]!;
        init => this[nameof(Persona)] = value;
    }

    public Context<string> Memory {
        get => (Context<string>)this[nameof(Memory)];
        init => this[nameof(Memory)] = value;
    }

    public Task Task {
        get => (Task)this[nameof(Task)];
        init => this[nameof(Task)] = value;
    }
}
