﻿namespace DotNetToolbox.AI.Agents;

public interface IAgentModel : IValidatable {
    public static readonly JsonSerializerOptions SerializerOptions = new() {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) },
    };
    string ModelId { get; }
    byte NumberOfRetries { get; }
    uint MaximumOutputTokens { get; }
    decimal Temperature { get; }
    decimal TokenProbabilityCutOff { get; }
    List<string> StopSequences { get; }
    bool ResponseIsStream { get; }
    bool RespondsAsJson { get; }
}
