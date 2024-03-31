namespace Sophia.Data.Chats;

public class ChatAgentOptionsEntity {
    [Range(0, AgentModel.MaximumRetries)]
    public byte NumberOfRetries { get; set; } = AgentModel.DefaultNumberOfRetries;
    public uint MaximumOutputTokens { get; set; } = AgentModel.DefaultMaximumOutputTokens;
    [Range(0, AgentModel.MaximumTemperature)]
    public decimal Temperature { get; set; } = AgentModel.DefaultTemperature;
    [Range(0, AgentModel.MaximumTokenProbabilityCutOff)]
    public decimal TokenProbabilityCutOff { get; set; }
    public List<string> StopSequences { get; set; } = [];
    public bool IsStreaming { get; set; }
    public bool RespondsAsJson { get; set; }
}
