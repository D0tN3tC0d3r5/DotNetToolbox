namespace DotNetToolbox.ConsoleApplication.Questions;

public interface IPromptFactory {
    YesOrNoPrompt CreateYesOrNoQuestion(string question, Action<YesOrNoOptions>? configure = null);
    MultipleChoicePrompt<TResult> CreateMultipleChoiceQuestion<TResult>(string question, Action<MultipleChoiceOptions<TResult>> configure);
    MultipleChoicePrompt CreateMultipleChoiceQuestion(string question, Action<MultipleChoiceOptions> configure);
    FreeTextPrompt CreateFreeTextQuestion(string question, Action<FreeTextOptions>? configure = null);
}
