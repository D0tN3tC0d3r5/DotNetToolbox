namespace DotNetToolbox.ConsoleApplication.Questions;

public class MultipleChoiceOptions
    : MultipleChoiceOptions<uint> {
    public override MultipleChoiceOptions AddChoice(uint result, string text, string? alias = null) {
        Choices.Add(new(Choices.Count, result, text, alias));
        return this;
    }

    public MultipleChoiceOptions AddChoice(string text, string? alias = null)
        => AddChoice((uint)(Choices.Count + 1), text, alias);
}

public class MultipleChoiceOptions<TResult>
    : QuestionOptions {
    public List<Choice<TResult>> Choices { get; } = [];
    public virtual MultipleChoiceOptions<TResult> AddChoice(TResult result, string text, string? alias = null) {
        Choices.Add(new(Choices.Count, result, text, alias));
        return this;
    }
}
