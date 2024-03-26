namespace Sophia.Services;

public class GetResponseRequest {
    public Guid ChatId { get; set; }
    public int? AgentNumber { get; set; }
    public string Message { get; set; } = string.Empty;
}
