namespace DotNetToolbox.ConsoleApplication.Questions;

public interface IQuestionFactory {
    Result<bool> YesOrNo(string text, Action<YesOrNo>? configure = null);
    Result<int> MultipleChoice(string text, Action<MultipleChoice> configure);
    Result<string> FreeText(string text, Action<FreeText>? configure = null);
}
