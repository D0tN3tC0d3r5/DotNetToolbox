namespace DotNetToolbox.ConsoleApplication.Questions;

public abstract class QuestionOptions {
    public bool EnforceAnswers { get; set; }
    public bool IsValid { get; protected set; }
}
