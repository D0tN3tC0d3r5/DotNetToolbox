namespace DotNetToolbox.Environment;

public interface IMultilinePromptBuilder {
    string Show();
    Task<string> ShowAsync(CancellationToken ct = default);
}
