namespace DotNetToolbox.AI.Anthropic;

public class AnthropicBackgroundAgent
    : BackgroundAgent<
        AnthropicBackgroundAgent,
        AnthropicAgentOptions,
        AnthropicChatRequest,
        AnthropicChatResponse> {
    private readonly AnthropicMapper<AnthropicBackgroundAgent> _mapper;

    public AnthropicBackgroundAgent(World world,
                                    AnthropicAgentOptions options,
                                    IPersona persona,
                                    IHttpClientProvider httpClientProvider,
                                    ILogger<AnthropicBackgroundAgent> logger)
        : base(world, options, persona, httpClientProvider, logger) {
        _mapper = new(this);
    }

    public AnthropicBackgroundAgent(IEnvironment environment,
                                    AnthropicAgentOptions options,
                                    IPersona persona,
                                    IHttpClientProvider httpClientProvider,
                                    ILogger<AnthropicBackgroundAgent> logger)
        : this(new World(environment), options, persona, httpClientProvider, logger) {
    }

    protected override AnthropicChatRequest CreateRequest(IConsumer source, IChat chat)
        => _mapper.CreateRequest(chat);

    protected override Message CreateResponseMessage(IChat chat, AnthropicChatResponse response)
        => _mapper.CreateResponseMessage(chat, response);
}
