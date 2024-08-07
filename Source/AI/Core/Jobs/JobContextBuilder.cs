namespace DotNetToolbox.AI.Jobs;

public class JobContextBuilder
    : IJobContextBuilder {
    private readonly IServiceProvider _services;
    private readonly IDictionary<string, object> _source;
    private string? _provider;

    private readonly Map _memory = [];
    private readonly Map<Asset> _assets = [];
    private readonly Map<Tool> _tools = [];

    private IDateTimeProvider _dateTime = DateTimeProvider.Default;
    private UserProfile? _userProfile;

    private IJob? _job;

    private JobContextBuilder(IServiceProvider services, IDictionary<string, object>? source) {
        _services = services;
        _source = source ?? new Map();
    }

    public static JobContextBuilder From(IServiceProvider services, IDictionary<string, object>? source = null)
        => new(services, source);

    public JobContextBuilder WithDateTimeFrom(IDateTimeProvider dateTime) {
        _dateTime = dateTime;
        return this;
    }

    public JobContextBuilder WithUser(UserProfile profile) {
        _userProfile = profile;
        return this;
    }

    public JobContextBuilder WithFact(string key, object value) {
        _memory[key] = value;
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

    public JobContextBuilder UsingAgentFrom(string provider) {
        _provider = provider;
        return this;
    }

    public JobContextBuilder ForJob(IJob job) {
        _job = job;
        return this;
    }

    public JobContext Build() {
        var agentFactory = _services.GetRequiredKeyedService<IAgentFactory>(_provider);
        return new(_services, _dateTime) {
            Job = _job ?? throw new InvalidOperationException("The Job is required"),
            Agent = agentFactory.Create(_provider),
            Memory = _memory,
            Assets = _assets,
            Tools = _tools,
            User = _userProfile,
        };
    }
}
