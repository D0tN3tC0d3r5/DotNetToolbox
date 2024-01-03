using DotNetToolbox.Options;

namespace DotNetToolbox.OpenAI.HttpProvider;

public class OpenAIOptions : HttpClientOptions<OpenAIOptions> {
    public override Uri? BaseAddress { get; set; } = new("https://api.openai.com/v1/");
}
