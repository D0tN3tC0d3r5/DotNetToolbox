namespace DotNetToolbox.OpenAI.HttpProvider;

public interface IOpenAIHttpClientProvider
    : IHttpClientProvider<OpenAIHttpClientOptionsBuilder, OpenAIOptions>;
