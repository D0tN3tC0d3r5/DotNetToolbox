namespace DotNetToolbox.AI.Models;

public interface IModel {
    string Id { get; }
    string Provider { get; }
    string Name { get; }
    uint MaximumContextSize { get; }
    uint MaximumOutputTokens { get; }
    DateOnly TrainingDataCutOff { get; }
}
