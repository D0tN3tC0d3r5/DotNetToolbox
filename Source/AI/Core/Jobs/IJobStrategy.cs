namespace DotNetToolbox.AI.Jobs;

public interface IJobStrategy<in TInput, out TOutput> {
    string Instructions { get; }
    IChat PrepareChat(JobContext context, TInput input);
    TOutput GetResult(IChat chat);
}
