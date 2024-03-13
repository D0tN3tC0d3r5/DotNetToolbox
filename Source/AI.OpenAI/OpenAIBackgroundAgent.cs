namespace DotNetToolbox.AI.OpenAI;

public class OpenAIBackgroundAgent
    : BackgroundAgent<OpenAIBackgroundAgent,
                      OpenAIAgentOptions,
                      OpenAIChatRequest,
                      OpenAIChatResponse> {
    private readonly OpenAIMapper<OpenAIBackgroundAgent> _mapper;

    public OpenAIBackgroundAgent(World world,
                                 OpenAIAgentOptions options,
                                 IPersona persona,
                                 IHttpClientProvider httpClientProvider,
                                 ILogger<OpenAIBackgroundAgent> logger)
        : base(world, options, persona, httpClientProvider, logger) {
        _mapper = new(this);
    }

    public OpenAIBackgroundAgent(IEnvironment environment,
                                 OpenAIAgentOptions options,
                                 IPersona persona,
                                 IHttpClientProvider httpClientProvider,
                                 ILogger<OpenAIBackgroundAgent> logger)
        : this(new World(environment), options, persona, httpClientProvider, logger) {
    }

    protected sealed override OpenAIChatRequest CreateRequest(IConsumer source, IChat chat)
        => _mapper.CreateRequest(chat);

    protected sealed override Message CreateResponseMessage(IChat chat, OpenAIChatResponse response)
        => _mapper.CreateResponseMessage(chat, response);
}
