namespace DotNetToolbox.AI.Jobs;

public class JobContext
    : Context {

    public JobContext(IDateTimeProvider? dateTime = null)
        : this(new Context(), dateTime) {
    }

    public JobContext(IContext source, IDateTimeProvider? dateTime = null)
        : base(source) {
        World = new(dateTime);
        Memory = [];
        Assets = [];
        Tools = [];
    }

    public World World {
        get => (World)this[nameof(World)]!;
        init => this[nameof(World)] = value;
    }

    public Context Memory {
        get => (Context)this[nameof(Memory)]!;
        init => this[nameof(Memory)] = value;
    }

    public Context<Asset> Assets {
        get => (Context<Asset>)this[nameof(Assets)]!;
        init => this[nameof(Assets)] = value;
    }

    public UserProfile? User {
        get => (UserProfile?)this[nameof(User)]!;
        init => this[nameof(User)] = value;
    }

    public Context<Tool> Tools {
        get => (Context<Tool>)this[nameof(Tools)]!;
        init => this[nameof(Tools)] = value;
    }

    public IJob Job {
        get => (IJob)this[nameof(Job)]!;
        set => this[nameof(Job)] = value;
    }

    public IAgent Agent {
        get => (IAgent)this[nameof(Agent)]!;
        set => this[nameof(Agent)] = value;
    }
}
