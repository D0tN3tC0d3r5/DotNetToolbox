namespace DotNetToolbox.ConsoleApplication.Questions;

public abstract class QuestionOptions<TResult>(QuestionPrompt prompt) {
    public QuestionPrompt Prompt { get; } = prompt;
    public virtual string InvalidChoiceMessage { get; set; } = "'{0}' is not a valid choice.";
    public abstract bool Validate(string input, out TResult result);
}
