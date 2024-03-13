namespace DotNetToolbox.AI.OpenAI;

public class OpenAIHttpClientOptions : HttpClientOptions {
    public override Uri? BaseAddress { get; set; } = new("https://api.openai.com");
}
