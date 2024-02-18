namespace DotNetToolbox.OpenAI.HttpProvider;

public class OpenAIOptions : HttpClientOptions {
    public override Uri? BaseAddress { get; set; } = new("https://api.openai.com/v1/");
}
