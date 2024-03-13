namespace DotNetToolbox.AI.Anthropic;

public class AnthropicQueuedAgent
    : QueuedAgent<
        AnthropicQueuedAgent,
        AnthropicAgentOptions,
        AnthropicChatRequest,
        AnthropicChatResponse> {
    private readonly AnthropicMapper<AnthropicQueuedAgent> _mapper;

    public AnthropicQueuedAgent(World world,
                                AnthropicAgentOptions options,
                                IPersona persona,
                                IHttpClientProvider httpClientProvider,
                                ILogger<AnthropicQueuedAgent> logger)
        : base(world, options, persona, httpClientProvider, logger) {
        _mapper = new(this);
    }

    public AnthropicQueuedAgent(IEnvironment environment,
                                AnthropicAgentOptions options,
                                IPersona persona,
                                IHttpClientProvider httpClientProvider,
                                ILogger<AnthropicQueuedAgent> logger)
        : this(new World(environment), options, persona, httpClientProvider, logger) {
    }

    protected override AnthropicChatRequest CreateRequest(IConsumer source, IChat chat)
        => _mapper.CreateRequest(chat);

    protected override Message CreateResponseMessage(IChat chat, AnthropicChatResponse response)
        => _mapper.CreateResponseMessage(chat, response);
}
