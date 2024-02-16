namespace DotNetToolbox.ConsoleApplication.Questions;

public class QuestionFactory(IOutput output, IInput input)
    : IQuestionFactory {
    public Result<bool> YesOrNo(string text, Action<YesOrNo>? configure = null) {
        var question = new YesOrNoQuestion(output, input, text, configure);
        return question.Ask();
    }

    public Result<int> MultipleChoice(string text, Action<MultipleChoice> configure) {
        var question = new MultipleChoiceQuestion(output, input, text, configure);
        return question.Ask();
    }

    public Result<string> FreeText(string text, Action<FreeText>? configure = null) {
        var question = new FreeTextQuestion(output, input, text, configure);
        return question.Ask();
    }
}
