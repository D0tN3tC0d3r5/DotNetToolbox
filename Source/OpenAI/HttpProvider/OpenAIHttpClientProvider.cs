namespace DotNetToolbox.OpenAI.HttpProvider;

public class OpenAIHttpClientProvider(IHttpClientFactory clientFactory, IOptions<OpenAIHttpClientOptions> options)
    : HttpClientProvider<OpenAIHttpClientOptionsBuilder, OpenAIHttpClientOptions>(clientFactory, options),
      IOpenAIHttpClientProvider {
}
