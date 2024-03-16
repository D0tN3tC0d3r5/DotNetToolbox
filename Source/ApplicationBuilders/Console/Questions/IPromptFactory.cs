namespace DotNetToolbox.ConsoleApplication.Questions;

public interface IPromptFactory {
    FreeTextPrompt CreateFreeTextQuestion(string question, Action<FreeTextOptions>? configure = null);
    YesOrNoPrompt CreateYesOrNoQuestion(string question, Action<YesOrNoOptions>? configure = null);
    MultipleChoicePrompt<TResult> CreateMultipleChoiceQuestion<TResult>(string question, Action<MultipleChoiceOptionsBuilder<TResult>> configure);
    MultipleChoicePrompt<TResult> CreateMultipleChoiceQuestion<TResult>(string question, IEnumerable<MultipleChoiceOption<TResult>> options);
    MultipleChoicePrompt<TResult> CreateMultipleChoiceQuestion<TResult>(string question, IEnumerable<TResult> options, bool displayIndex = false);
    MultipleChoicePrompt<uint> CreateMultipleChoiceQuestion(string question, Action<MultipleChoiceOptionsBuilder<uint>> configure);
    MultipleChoicePrompt<uint> CreateMultipleChoiceQuestion(string question, IEnumerable<MultipleChoiceOption<uint>> options);
    MultipleChoicePrompt<string> CreateMultipleChoiceQuestion(string question, Action<MultipleChoiceOptionsBuilder<string>> configure);
    MultipleChoicePrompt<string> CreateMultipleChoiceQuestion(string question, IEnumerable<MultipleChoiceOption<string>> options);
    MultipleChoicePrompt<string> CreateMultipleChoiceQuestion(string question, IEnumerable<string> options, bool displayIndex = false);
}
