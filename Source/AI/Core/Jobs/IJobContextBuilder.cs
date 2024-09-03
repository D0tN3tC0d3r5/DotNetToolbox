
namespace DotNetToolbox.AI.Jobs;

public interface IJobContextBuilderFactory {
    IJobContextBuilder CreateFrom(IEnumerable<KeyValuePair<string, object>> source);
    IJobContextBuilder Create();
}

public interface IJobContextBuilder {
    IJobContextBuilder WithAgentFrom(string provider);
    IJobContextBuilder WithAsset(Asset asset);
    IJobContextBuilder WithDateTimeFrom(IDateTimeProvider dateTime);
    IJobContextBuilder WithFact(string key, object value);
    IJobContextBuilder WithTool(Tool tool);
    IJobContextBuilder WithUser(UserProfile profile);

    JobContext Build();
}
