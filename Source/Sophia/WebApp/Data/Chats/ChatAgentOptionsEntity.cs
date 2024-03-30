namespace Sophia.WebApp.Data.Chats;

public class ChatAgentOptionsEntity {
    [Required]
    [MaxLength(50)]
    public string ModelId { get; set; } = default!;
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

    public ChatAgentOptionsData ToDto(ModelData[] models)
        => new() {
                     Model = models.First(i => i.Id == ModelId),
                     NumberOfRetries = NumberOfRetries,
                     MaximumOutputTokens = MaximumOutputTokens,
                     Temperature = Temperature,
                     TokenProbabilityCutOff = TokenProbabilityCutOff,
                     StopSequences = [..StopSequences],
                     IsStreaming = IsStreaming,
                     RespondsAsJson = RespondsAsJson,
                 };
}
