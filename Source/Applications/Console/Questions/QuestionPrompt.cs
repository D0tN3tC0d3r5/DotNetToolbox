namespace DotNetToolbox.ConsoleApplication.Questions;

public abstract class Prompt(ISystemEnvironment environment) {
    protected IOutput Output { get; } = environment.ConsoleOutput;
    protected IInput Input { get; } = environment.ConsoleInput;
    protected virtual void ShowPrompt() => Output.WritePrompt();
}

public abstract class QuestionPrompt<TResult>(string question, ISystemEnvironment environment)
    : Prompt(environment) {
    protected string Question { get; set; } = IsNotNullOrWhiteSpace(question);
    public abstract bool Validate(string input, out TResult result);

    public virtual TResult Ask() {
        while (true) {
            Output.WriteLine(Question);
            ShowPrompt();
            var answer = Input.ReadLine()?.Trim() ?? string.Empty;
            if (Validate(answer, out var result)) return result;
            Output.WriteLine(InvalidAnswerMessage, answer);
        }
    }

    protected virtual string InvalidAnswerMessage => "'{0}' is not a valid answer.";
}
