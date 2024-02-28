namespace DotNetToolbox.ConsoleApplication.Questions;

public interface IQuestionFactory {
    bool YesOrNo(string question, Action<YesOrNoOptions>? configure = null);
    uint MultipleChoice(string question, Action<MultipleChoiceOptions> configure);
    string FreeText(string question, Action<FreeTextOptions>? configure = null);
}
