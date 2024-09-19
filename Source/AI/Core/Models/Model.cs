namespace DotNetToolbox.AI.Models;

public class Model(uint id)
    : IModel {
    public uint Id { get; init; } = id;
    public string Provider { get; init; } = string.Empty;
    public string Key { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public uint MaximumContextSize { get; init; }
    public uint MaximumOutputTokens { get; init; }
    public DateOnly? TrainingDataCutOff { get; init; }
}
