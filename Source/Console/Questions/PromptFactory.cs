namespace DotNetToolbox.ConsoleApplication.Questions;

public class PromptFactory(IEnvironment environment)
    : IPromptFactory {
    public YesOrNoPrompt CreateYesOrNoQuestion(string question, Action<YesOrNoOptions>? configure = null) {
        var options = new YesOrNoOptions();
        configure?.Invoke(options);
        return new(question, environment, options);
    }

    public MultipleChoicePrompt<TResult> CreateMultipleChoiceQuestion<TResult>(string question, Action<MultipleChoiceOptions<TResult>> configure) {
        var options = new MultipleChoiceOptions<TResult>();
        configure?.Invoke(options);
        return new(question, environment, options);
    }

    public MultipleChoicePrompt CreateMultipleChoiceQuestion(string question, Action<MultipleChoiceOptions> configure) {
        var options = new MultipleChoiceOptions();
        configure?.Invoke(options);
        return new(question, environment, options);
    }

    public FreeTextPrompt CreateFreeTextQuestion(string question, Action<FreeTextOptions>? configure = null) {
        var options = new FreeTextOptions();
        configure?.Invoke(options);
        return new FreeTextPrompt(question, environment, options);
    }
}
