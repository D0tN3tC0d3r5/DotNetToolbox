namespace DotNetToolbox.ConsoleApplication.Questions;

public class MultipleChoice : QuestionOptions {
    public List<AnswerOption> Choices { get; set; } = [];

    public Result<int> Check(string input) {
        if (int.TryParse(input, out var selectedIndex)
          && selectedIndex > 0
          && selectedIndex <= Choices.Count) {
            return Success(selectedIndex - 1); // Return index in Choices
        }

        foreach (var choice in Choices.AsIndexed()) {
            if (ChoiceIsSelected(choice.Value, input))
                return Success((int)choice.Index);
        }

        return EnforceAnswers
                   ? Invalid(-1, $"{input} is not a valid option.")
                   : Success(-1); // -1 or another value to indicate "no selection"
    }

    private static bool ChoiceIsSelected(AnswerOption choice, string input)
        => choice.Matches(input);

    // Helper method to add choices for fluency
    public MultipleChoice AddChoice(string text, string? shortcut = null) {
        Choices.Add(new(text, shortcut));
        return this;
    }
}
