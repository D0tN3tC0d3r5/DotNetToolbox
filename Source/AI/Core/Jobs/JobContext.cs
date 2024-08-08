namespace DotNetToolbox.AI.Jobs;

public class JobContext
    : Context {
    public JobContext(IServiceProvider services, IDateTimeProvider? dateTime = null)
        : this(services, new Context(services), dateTime) {
    }

    public JobContext(IServiceProvider services, IContext source, IDateTimeProvider? dateTime = null)
        : base(services, source) {
        World = new(dateTime);
        Memory = [];
        Assets = [];
        Tools = [];
    }

    public World World {
        get => (World)this[nameof(World)];
        init => this[nameof(World)] = value;
    }

    public Map Memory {
        get => (Map)this[nameof(Memory)];
        init => this[nameof(Memory)] = value;
    }

    public Map<Asset> Assets {
        get => (Map<Asset>)this[nameof(Assets)];
        init => this[nameof(Assets)] = value;
    }

    public UserProfile? User {
        get => (UserProfile?)this[nameof(User)]!;
        init => this[nameof(User)] = value;
    }

    public Map<Tool> Tools {
        get => (Map<Tool>)this[nameof(Tools)];
        init => this[nameof(Tools)] = value;
    }

    public IAgent Agent {
        get => (IAgent)this[nameof(Agent)];
        init => this[nameof(Agent)] = value;
    }

    public IJob Job {
        get => (IJob)this[nameof(Job)];
        set => this[nameof(Job)] = value;
    }
}
