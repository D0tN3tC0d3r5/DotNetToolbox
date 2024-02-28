namespace DotNetToolbox.ConsoleApplication.Questions;

public abstract class QuestionPrompt(string question, IEnvironment environment) {
    protected IOutput Output { get; } = environment.Output;
    protected IInput Input { get; } = environment.Input;
    protected string Question { get; set; } = IsNotNullOrWhiteSpace(question);
    protected virtual void ShowPrompt() => Output.WritePrompt();

    internal List<AnswerOption> Choices { get; } = [];
}

public abstract class QuestionPrompt<TResult, TOptions>(string question, IEnvironment environment, Action<TOptions> configure)
    : QuestionPrompt(question, environment) where TOptions : QuestionOptions<TResult> {
    public Action<TOptions> ConfigureOptions { get; } = IsNotNull(configure);

    public virtual TResult Ask() {
        var options = CreateInstance.Of<TOptions>(this);
        ConfigureOptions(options);
        IsNotEmpty(Choices);
        while (true) {
            Output.WriteLine(Question);
            ShowPrompt();
            var answer = Input.ReadLine()?.Trim() ?? string.Empty;
            if (options.Validate(answer, out var result)) return result;
            Output.WriteLine(options.InvalidChoiceMessage, answer);
        }
    }
}
