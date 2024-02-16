namespace DotNetToolbox.ConsoleApplication.Questions;

public abstract class Question<TValue>(IOutput output, IInput input, string text) {
    protected IOutput Output { get; } = IsNotNull(output);
    protected IInput Input { get; } = IsNotNull(input);
    public string Text { get; protected set; } = IsNotNullOrWhiteSpace(text);

    public abstract Result<TValue> Ask();
}
