
namespace DotNetToolbox.AI.Jobs;

public interface IJobContextBuilderFactory {
    IJobContextBuilder CreateFrom(IEnumerable<KeyValuePair<string, object>> source);
    IJobContextBuilder Create();
}

public interface IJobContextBuilder {
    IJobContextBuilder WithModel(Model model);
    IJobContextBuilder WithAsset(Asset asset);
    IJobContextBuilder WithDateTimeFrom(IDateTimeProvider dateTime);
    IJobContextBuilder WithFact(string key, string value);
    IJobContextBuilder WithTool(Tool tool);
    IJobContextBuilder WithUser(UserProfile profile);
    IJobContextBuilder WithInput(object input);

    JobContext Build();
}
