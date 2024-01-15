namespace DotNetToolbox.OpenAI.HttpProvider;

public class OpenAIHttpClientProvider(IHttpClientFactory clientFactory, IOptions<OpenAIOptions> options)
    : HttpClientProvider<OpenAIHttpClientOptionsBuilder, OpenAIOptions>(clientFactory, options),
      IOpenAIHttpClientProvider;
