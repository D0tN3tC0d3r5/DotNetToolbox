namespace DotNetToolbox.AI.Jobs;

public interface IJobStrategy<in TInput, out TOutput> {
    string Instructions { get; }

    void AddPrompt(IChat chat, TInput input, IJobContext context);
    TOutput GetResult(IChat chat, IJobContext context);
}
