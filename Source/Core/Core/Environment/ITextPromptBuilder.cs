namespace DotNetToolbox.Environment;

public interface IPromptBuilder
    : ITextPromptBuilder<string> {
}

public interface ITextPromptBuilder<TValue> {
    TextPromptBuilder<TValue> For(string name);
    TextPromptBuilder<TValue> AsRequired();
    TextPromptBuilder<TValue> WithDefault(TValue defaultValue);
    TextPromptBuilder<TValue> UseMask(char? maskChar);
    TextPromptBuilder<TValue> ConvertWith(Func<TValue, string> converter);
    TextPromptBuilder<TValue> AddChoice(TValue choice);
    TextPromptBuilder<TValue> AddChoices(params TValue[] otherChoices);
    TextPromptBuilder<TValue> Validate(Func<TValue, Result> validate);
    TextPromptBuilder<TValue> Validate(Func<TValue, bool> validate, string errorMessage);
    TValue Show();
}
