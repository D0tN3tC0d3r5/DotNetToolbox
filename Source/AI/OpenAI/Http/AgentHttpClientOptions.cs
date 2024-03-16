namespace DotNetToolbox.AI.OpenAI.Http;

public class AgentHttpClientOptions : HttpClientOptions {
    public override Uri? BaseAddress { get; set; } = new("https://api.openai.com");
}
