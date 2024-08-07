
namespace DotNetToolbox.AI.Jobs;

public interface IJobContextBuilder {
    JobContextBuilder UsingAgentFrom(string provider);
    JobContextBuilder WithAsset(Asset asset);
    JobContextBuilder WithDateTimeFrom(IDateTimeProvider dateTime);
    JobContextBuilder WithFact(string key, object value);
    JobContextBuilder ForJob(IJob job);
    JobContextBuilder WithTool(Tool tool);
    JobContextBuilder WithUser(UserProfile profile);

    JobContext Build();
}
