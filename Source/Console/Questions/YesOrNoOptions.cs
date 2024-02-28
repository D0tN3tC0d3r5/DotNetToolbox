namespace DotNetToolbox.ConsoleApplication.Questions;

public class YesOrNoOptions : QuestionOptions<bool> {
    public YesOrNoOptions(QuestionPrompt prompt)
        : base(prompt) {
        prompt.Choices.Add(new("Yes", "Y"));
        prompt.Choices.Add(new("No", "N"));
    }

    public AnswerOption Yes {
        private get => Prompt.Choices[0];
        set => Prompt.Choices[0] = value;
    }

    public AnswerOption No {
        private get => Prompt.Choices[1];
        set => Prompt.Choices[1] = value;
    }

    public override bool Validate(string input, out bool result) {
        result = default;
        if (Yes.Matches(input)) {
            result = true;
            return true;
        }
        if (!No.Matches(input)) return false;
        result = false;
        return true;
    }
}
