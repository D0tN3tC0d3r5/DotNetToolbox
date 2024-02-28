namespace DotNetToolbox.ConsoleApplication.Questions;

public class FreeTextPrompt(string question, IEnvironment environment, Action<FreeTextOptions>? configure = null)
    : QuestionPrompt<string, FreeTextOptions>(question, environment, configure ?? (_ => { })) {
}
