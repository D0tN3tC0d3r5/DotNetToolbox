namespace DotNetToolbox.AI.OpenAI;

public abstract class OpenAIAgent<TAgent>
    : Agent<TAgent,
            OpenAIAgentOptions,
            OpenAIChatRequest,
            OpenAIChatResponse>
    where TAgent : OpenAIAgent<TAgent> {

    private readonly OpenAIMapper<TAgent> _mapper;

    protected OpenAIAgent(World world,
                          OpenAIAgentOptions options,
                          IPersona persona,
                          IHttpClientProvider httpClientProvider,
                          ILogger<TAgent> logger)
        : base(world, options, persona, httpClientProvider, logger) {
        _mapper = new((TAgent)this);
    }

    protected OpenAIAgent(IEnvironment environment,
                          OpenAIAgentOptions options,
                          IPersona persona,
                          IHttpClientProvider httpClientProvider,
                          ILogger<TAgent> logger)
        : this(new World(environment), options, persona, httpClientProvider, logger) {
    }

    protected sealed override OpenAIChatRequest CreateRequest(IConsumer source, IChat chat)
        => _mapper.CreateRequest(chat);

    protected sealed override Message CreateResponseMessage(IChat chat, OpenAIChatResponse response)
        => _mapper.CreateResponseMessage(chat, response);
}
