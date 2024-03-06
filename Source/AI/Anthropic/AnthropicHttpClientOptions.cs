namespace DotNetToolbox.AI.Anthropic;

public class AnthropicHttpClientOptions : HttpClientOptions {
    public override Uri? BaseAddress { get; set; } = new("https://api.anthropic.com/");
}
