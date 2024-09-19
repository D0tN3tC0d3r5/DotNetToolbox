namespace DotNetToolbox.AI.Models;

public interface IModel {
    uint Id { get; }
    string Provider { get; }
    string Key { get; }
    string Name { get; }
    uint MaximumContextSize { get; }
    uint MaximumOutputTokens { get; }
    DateOnly? TrainingDataCutOff { get; }
}
