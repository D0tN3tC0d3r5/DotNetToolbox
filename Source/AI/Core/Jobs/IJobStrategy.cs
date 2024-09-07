namespace DotNetToolbox.AI.Jobs;

public interface IJobStrategy<in TInput, out TOutput> {
    string Instructions { get; }

    void AddPrompt(IMessages chat, TInput input, IJobContext context);
    TOutput GetResult(IMessages chat, IJobContext context);
}
