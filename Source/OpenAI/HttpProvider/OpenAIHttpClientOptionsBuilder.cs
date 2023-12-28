namespace DotNetToolbox.OpenAI.HttpProvider;

public class OpenAIHttpClientOptionsBuilder(OpenAIHttpClientOptions? options = null)
    : HttpClientOptionsBuilder<OpenAIHttpClientOptions>(options) {
}
