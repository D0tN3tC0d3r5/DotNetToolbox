namespace AI.Sample.Services;

public class AIService(IModelHandler modelHandler, IAgentHandler agentHandler, IAgentFactory aiAgentFactory, ILogger<AIService> logger)
    : IAIService {
    private readonly IModelHandler _modelHandler = modelHandler;
    private readonly IAgentHandler _agentHandler = agentHandler;
    private readonly IAgentFactory _aiAgentFactory = aiAgentFactory;
    private readonly ILogger<AIService> _logger = logger;

    public Task<string> GetNextQuestion(PersonaEntity persona) {
        try {
            var appModel = _modelHandler.Selected ?? throw new InvalidOperationException("No AI model selected."); ;
            var aiAgent = _aiAgentFactory.Create(appModel.Provider!.NormalizedName);
            aiAgent.Settings.Model = new DotNetToolbox.AI.Models.Model() {
                Name = appModel.Name,
                Id = appModel.Key,
                MaximumContextSize = appModel.MaximumContextSize,
                MaximumOutputTokens = appModel.MaximumOutputTokens,
                TrainingDataCutOff = appModel.TrainingDataCutOff,
            };
            aiAgent.Persona = new DotNetToolbox.AI.Agents.Persona();

            var chat = new Chat(Guid.NewGuid().ToString(), new Context());
            // Add system message to set up the context
            var message = new Message(MessageRole.System, """
                                                          You are an AI assistant helping to generate a persona.
                                                          Ask relevant questions to gather information for creating a detailed persona.
                                                          """);
            chat.Messages.Add(message);

            // Add user message with current persona information
            var userMessage = $"""
                               I'm creating a persona with the following details:
                               Name: {persona.Name}
                               Primary Role: {persona.PrimaryRole}
                               Intended Use: {persona.IntendedUse}
                               """;
            if (persona.AdditionalInformation.Any()) {
                userMessage += "Additional Information:\n";
                foreach (var query in persona.AdditionalInformation) {
                    userMessage += $"""
                                    Q: {query.Question}
                                    A: {query.Answer}
                                    """;
                }
            }
            userMessage += "What's the next question you'd like to ask to gather more information for this persona?";
            chat.Messages.Add(new Message(MessageRole.User, userMessage));

            //var jobContext = new JobContext() {
            //    World = new World(),
            //    Agent = aiAgent,
            //    User = new UserProfile(),
            //};

            //var job = new PersonaGenerationJob(jobContext);

            //var result = await job.Execute(chat, CancellationToken.None);
            //if (result.HasException) {
            //    throw new Exception("Failed to generate next question: " + result.Exception.Message);
            //}

            //return result.Value;

            return Task.FromResult(string.Empty);
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Error generating next question for persona {PersonaName}", persona.Name);
            throw;
        }
    }

    //public async Task<string> GeneratePrompt(PersonaEntity persona) {
    //    try {
    //        var appAgent = _agentHandler.Selected ?? throw new InvalidOperationException("No AI agent selected.");
    //        var aiAgent = _aiAgentFactory.Create(appAgent.ProviderId);

    //        var chat = new Chat(Guid.NewGuid().ToString(), new Context());

    //        // Add system message to set up the context
    //        chat.Messages.Add(new Message(MessageRole.System, "You are an AI assistant tasked with generating a detailed prompt for an AI persona based on the provided information."));

    //        // Add user message with all persona information
    //        var userMessage = $"Generate a detailed prompt for an AI persona with the following information:\n" +
    //                          $"Name: {persona.Name}\n" +
    //                          $"Primary Role: {persona.PrimaryRole}\n" +
    //                          $"Intended Use: {persona.IntendedUse}\n" +
    //                          "Additional Information:\n";
    //        foreach (var query in persona.AdditionalInformation) {
    //            userMessage += $"Q: {query.Question}\nA: {query.Answer}\n";
    //        }
    //        userMessage += "Please create a comprehensive prompt that captures all aspects of this persona, including its role, expertise, communication style, and any other relevant characteristics.";
    //        chat.Messages.Add(new Message(MessageRole.User, userMessage));

    //        var job = new PromptGenerationJob(new JobContext(new ServiceCollection().BuildServiceProvider())) {
    //            World = new World(),
    //            Persona = new AI.Agents.Persona(),
    //            Agent = aiAgent
    //        };

    //        var result = await job.Execute(chat, CancellationToken.None);
    //        if (result.HasException) {
    //            throw new Exception("Failed to generate prompt: " + result.Exception.Message);
    //        }

    //        return result.Value;
    //    }
    //    catch (Exception ex) {
    //        _logger.LogError(ex, "Error generating prompt for persona {PersonaName}", persona.Name);
    //        throw;
    //    }
    //}
}
