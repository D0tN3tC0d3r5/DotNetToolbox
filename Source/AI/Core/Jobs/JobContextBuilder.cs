namespace DotNetToolbox.AI.Jobs;

public class JobContextBuilder {
    private IDateTimeProvider? _dateTime;
    private UserProfile? _userProfile;
    private readonly Context _memory = [];
    private readonly Context<Asset> _assets = [];
    private readonly Context<Tool> _tools = [];
    private IJob? _job;
    private IAgent? _agent;

    public JobContextBuilder WithDateTimeProvider(IDateTimeProvider dateTime) {
        _dateTime = dateTime;
        return this;
    }

    public JobContextBuilder WithUserProfile(UserProfile profile) {
        _userProfile = profile;
        return this;
    }

    public JobContextBuilder WithFact(string key, object memory) {
        _memory[key] = memory;
        return this;
    }

    public JobContextBuilder WithAsset(Asset asset) {
        _assets[asset.Id] = asset;
        return this;
    }

    public JobContextBuilder WithTool(Tool tool) {
        _tools[tool.Id] = tool;
        return this;
    }

    public JobContextBuilder WithAgent(IAgent agent) {
        _agent = agent;
        return this;
    }

    public JobContextBuilder WithJob(IJob job) {
        _job = job;
        return this;
    }

    public JobContext Build()
        => new(_dateTime) {
            Agent = _agent ?? throw new InvalidOperationException("The Agent is required"),
            Job = _job ?? throw new InvalidOperationException("The Job is required"),
            Memory = _memory,
            Assets = _assets,
            Tools = _tools,
            User = _userProfile,
        };
}
