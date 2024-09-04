namespace DotNetToolbox.AI.Models;

public interface IModel {
    string Id { get; }
    string ProviderId { get; }
    string Name { get; }
    uint MaximumContextSize { get; }
    uint MaximumOutputTokens { get; }
    DateOnly TrainingDataCutOff { get; }
}
