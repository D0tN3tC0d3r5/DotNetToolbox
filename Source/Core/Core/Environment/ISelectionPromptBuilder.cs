namespace DotNetToolbox.Environment;

public interface ISelectionPromptBuilder<TValue>
    where TValue : notnull {
    SelectionPromptBuilder<TValue> WithDefault(TValue defaultValue);
    SelectionPromptBuilder<TValue> ConvertWith(Func<TValue, string> converter);
    SelectionPromptBuilder<TValue> AddChoice(TValue choice);
    SelectionPromptBuilder<TValue> AddChoices(params TValue[] otherChoices);
    SelectionPromptBuilder<TValue> ShowResult();
    TValue Show();
    Task<TValue> ShowAsync(CancellationToken ct = default);
}
