﻿namespace DotNetToolbox.Environment;

public interface IPromptBuilder
    : ITextPromptBuilder<string>;

public interface ITextPromptBuilder<TValue> {
    TextPromptBuilder<TValue> WithDefault(TValue defaultValue);
    TextPromptBuilder<TValue> UseMask(char? maskChar);
    TextPromptBuilder<TValue> ConvertWith(Func<TValue, string> converter);
    TextPromptBuilder<TValue> AddChoices(TValue choice, params TValue[] otherChoices);
    TextPromptBuilder<TValue> Validate(Func<TValue, Result> validate);
    TValue Show();
    Task<TValue> ShowAsync(CancellationToken ct = default);
}