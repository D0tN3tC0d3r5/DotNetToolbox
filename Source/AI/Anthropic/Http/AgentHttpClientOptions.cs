namespace DotNetToolbox.AI.Anthropic.Http;

public class AgentHttpClientOptions : HttpClientOptions {
    public override Uri? BaseAddress { get; set; } = new("https://api.anthropic.com");
}
