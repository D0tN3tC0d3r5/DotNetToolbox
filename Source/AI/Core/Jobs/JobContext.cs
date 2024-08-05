namespace DotNetToolbox.AI.Jobs;

public class JobContext
    : Context {
    public JobContext(IDateTimeProvider? dateTime = null) {
        this[nameof(World)] = new World(dateTime);
        this[nameof(Memory)] = new Context();
        this[nameof(Assets)] = new Context<Asset>();
    }

    public World World => (World)this[nameof(World)]!;

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

    public required IAgent Agent {
        get => (IAgent)this[nameof(Agent)]!;
        init => this[nameof(Agent)] = value;
    }

    public required Context<Tool> Tools {
        get => (Context<Tool>)this[nameof(Tools)]!;
        init => this[nameof(Tools)] = value;
    }

    public required IJob Job {
        get => (IJob)this[nameof(Job)]!;
        set => this[nameof(Job)] = value;
    }
}
