using DotNetToolbox.Options;

namespace DotNetToolbox.OpenAI.HttpProvider;

public class OpenAIHttpClientOptions : HttpClientOptions<OpenAIHttpClientOptions>, INamedOptions<OpenAIHttpClientOptions> {
    public static string SectionName => "OpenAI";
    public override Uri? BaseAddress { get; set; } = new("https://api.openai.com/v1/");
}
