namespace DotNetToolbox.AI.Models;

public class Model
    : IModel {
    public required string Id { get; init; }
    public required string ProviderId { get; init; }
    public required string Name { get; init; }
    public uint MaximumContextSize { get; init; }
    public uint MaximumOutputTokens { get; init; }
    public DateOnly TrainingDataCutOff { get; init; }
}
