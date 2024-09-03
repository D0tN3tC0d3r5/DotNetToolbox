namespace DotNetToolbox.AI.Jobs;

public class JobContextBuilder(IServiceProvider services)
    : IJobContextBuilder,
      IJobContextBuilderFactory {

    private IEnumerable<KeyValuePair<string, object>>? _source;
    private IDateTimeProvider _dateTime = DateTimeProvider.Default;
    private UserProfile? _userProfile;
    private readonly Map _memory = [];
    private readonly Map<Asset> _assets = [];
    private readonly Map<Tool> _tools = [];
    private string? _provider;

    public IJobContextBuilder CreateFrom(IEnumerable<KeyValuePair<string, object>> source) {
        _source = IsNotNull(source);
        return this;
    }
    public IJobContextBuilder Create() => this;

    public IJobContextBuilder WithAgentFrom(string provider) {
        _provider = provider;
        return this;
    }

    public IJobContextBuilder WithDateTimeFrom(IDateTimeProvider dateTime) {
        _dateTime = dateTime;
        return this;
    }

    public IJobContextBuilder WithUser(UserProfile profile) {
        _userProfile = profile;
        return this;
    }

    public IJobContextBuilder WithFact(string key, object value) {
        _memory[key] = value;
        return this;
    }

    public IJobContextBuilder WithAsset(Asset asset) {
        _assets[asset.Id] = asset;
        return this;
    }

    public IJobContextBuilder WithTool(Tool tool) {
        _tools[tool.Id] = tool;
        return this;
    }

    public JobContext Build() {
        var agentFactory = services.GetRequiredKeyedService<IAgentFactory>(_provider);
        return new(_source) {
            World = new World(_dateTime),
            Agent = agentFactory.Create(_provider),
            Memory = _memory,
            Assets = _assets,
            Tools = _tools,
            User = _userProfile,
        };
    }
}
