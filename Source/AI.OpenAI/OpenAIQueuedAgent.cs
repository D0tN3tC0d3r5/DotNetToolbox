namespace DotNetToolbox.AI.OpenAI;

public class OpenAIQueuedAgent
    : QueuedAgent<OpenAIQueuedAgent,
                  OpenAIAgentOptions,
                  OpenAIChatRequest,
                  OpenAIChatResponse> {
    private readonly OpenAIMapper<OpenAIQueuedAgent> _mapper;

    public OpenAIQueuedAgent(World world,
                             OpenAIAgentOptions options,
                             IPersona persona,
                             IHttpClientProvider httpClientProvider,
                             ILogger<OpenAIQueuedAgent> logger)
        : base(world, options, persona, httpClientProvider, logger) {
        _mapper = new(this);
    }

    public OpenAIQueuedAgent(IEnvironment environment,
                             OpenAIAgentOptions options,
                             IPersona persona,
                             IHttpClientProvider httpClientProvider,
                             ILogger<OpenAIQueuedAgent> logger)
        : this(new World(environment), options, persona, httpClientProvider, logger) {
    }

    protected sealed override OpenAIChatRequest CreateRequest(IConsumer source, IChat chat)
        => _mapper.CreateRequest(chat);

    protected sealed override Message CreateResponseMessage(IChat chat, OpenAIChatResponse response)
        => _mapper.CreateResponseMessage(chat, response);
}
