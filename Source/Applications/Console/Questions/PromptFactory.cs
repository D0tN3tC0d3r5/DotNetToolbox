namespace DotNetToolbox.ConsoleApplication.Questions;

public class PromptFactory(IApplicationEnvironment environment)
    : IPromptFactory {
    public FreeTextPrompt CreateFreeTextQuestion(string question, Action<FreeTextOptions>? configure = null) {
        var options = new FreeTextOptions();
        configure?.Invoke(options);
        return new(question, environment, options);
    }

    public YesOrNoPrompt CreateYesOrNoQuestion(string question, Action<YesOrNoOptions>? configure = null) {
        var options = new YesOrNoOptions();
        configure?.Invoke(options);
        return new(question, environment, options);
    }

    public MultipleChoicePrompt<TResult> CreateMultipleChoiceQuestion<TResult>(string question, Action<MultipleChoiceOptionsBuilder<TResult>> configure) {
        var options = new MultipleChoiceOptionsBuilder<TResult>();
        IsNotNull(configure)(options);
        return new(question, environment, options.Build());
    }

    public MultipleChoicePrompt<TResult> CreateMultipleChoiceQuestion<TResult>(string question, IEnumerable<MultipleChoiceOption<TResult>> options)
        => new(question, environment, options);

    public MultipleChoicePrompt<TResult> CreateMultipleChoiceQuestion<TResult>(string question, IEnumerable<TResult> options, bool displayIndex = false)
        => CreateMultipleChoiceQuestion(question, options.AsIndexed().ToArray(o => new MultipleChoiceOption<TResult>(o.Index, o.Value, $"{o.Value}", displayIndex)));

    public MultipleChoicePrompt<uint> CreateMultipleChoiceQuestion(string question, Action<MultipleChoiceOptionsBuilder<uint>> configure)
        => CreateMultipleChoiceQuestion<uint>(question, configure);

    public MultipleChoicePrompt<uint> CreateMultipleChoiceQuestion(string question, IEnumerable<MultipleChoiceOption<uint>> options)
        => CreateMultipleChoiceQuestion<uint>(question, options);

    public MultipleChoicePrompt<string> CreateMultipleChoiceQuestion(string question, Action<MultipleChoiceOptionsBuilder<string>> configure)
        => CreateMultipleChoiceQuestion<string>(question, configure);

    public MultipleChoicePrompt<string> CreateMultipleChoiceQuestion(string question, IEnumerable<MultipleChoiceOption<string>> options)
        => CreateMultipleChoiceQuestion<string>(question, options);

    public MultipleChoicePrompt<string> CreateMultipleChoiceQuestion(string question, IEnumerable<string> options, bool displayIndex = false)
        => CreateMultipleChoiceQuestion<string>(question, options, displayIndex);
}
