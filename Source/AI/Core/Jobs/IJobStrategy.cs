namespace DotNetToolbox.AI.Jobs;

public interface IJobStrategy<in TInput, out TOutput> {
    string Instructions { get; }

    void AddPrompt(IChat chat, TInput input);
    TOutput GetResult(IChat chat);
}
