namespace DotNetToolbox.AI.Models;

public class Model(string id)
    : IModel {
    public string Id { get; init; } = IsNotNullOrWhiteSpace(id);
    public string Provider { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public uint MaximumContextSize { get; init; }
    public uint MaximumOutputTokens { get; init; }
    public DateOnly TrainingDataCutOff { get; init; }
}
