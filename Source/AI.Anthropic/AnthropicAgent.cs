namespace DotNetToolbox.AI.Anthropic;

public interface IAnthropicAgent
    : IAgent<AnthropicAgentOptions>;

public abstract class AnthropicAgent<TAgent>
    : Agent<TAgent, AnthropicAgentOptions, AnthropicChatRequest, AnthropicChatResponse>,
      IAnthropicAgent
    where TAgent : AnthropicAgent<TAgent> {

    private readonly AnthropicMapper<TAgent> _mapper;

    protected AnthropicAgent(World world,
                             AnthropicAgentOptions options,
                             IPersona persona,
                             IHttpClientProvider httpClientProvider,
                             ILogger<TAgent> logger)
        : base(world, options, persona, httpClientProvider, logger) {
        _mapper = new((TAgent)this);
    }

    protected AnthropicAgent(IEnvironment environment,
                             AnthropicAgentOptions options,
                             IPersona persona,
                             IHttpClientProvider httpClientProvider,
                             ILogger<TAgent> logger)
        : this(new World(environment), options, persona, httpClientProvider, logger) {
    }

    protected sealed override AnthropicChatRequest CreateRequest(IConsumer source, IChat chat)
        => _mapper.CreateRequest(chat);

    protected sealed override Message CreateResponseMessage(IChat chat, AnthropicChatResponse response)
        => _mapper.CreateResponseMessage(chat, response);
}
