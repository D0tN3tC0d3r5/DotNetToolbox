namespace DotNetToolbox.ConsoleApplication.Questions;

public class QuestionFactory(IEnvironment environment)
    : IQuestionFactory {
    public bool YesOrNo(string question, Action<YesOrNoOptions>? configure = null) {
        var prompt = new YesOrNoPrompt(question, environment, configure);
        return prompt.Ask();
    }

    public uint MultipleChoice(string question, Action<MultipleChoiceOptions> configure) {
        var prompt = new MultipleChoicePrompt(question, environment, configure);
        return prompt.Ask();
    }

    public string FreeText(string question, Action<FreeTextOptions>? configure = null) {
        var prompt = new FreeTextPrompt(question, environment, configure);
        return prompt.Ask();
    }
}
