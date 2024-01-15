namespace DotNetToolbox.OpenAI.HttpProvider;

public class OpenAIHttpClientOptionsBuilder(OpenAIOptions? options = null)
    : HttpClientOptionsBuilder<OpenAIOptions>(options);
