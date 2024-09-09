using Task = System.Threading.Tasks.Task;

namespace AI.Sample.Personas.Handlers;

public class PersonaHandler(IServiceProvider services, ILogger<PersonaHandler> logger)
    : IPersonaHandler {
    private readonly IModelHandler _modelHandler = services.GetRequiredService<IModelHandler>();
    private readonly IUserProfileHandler _userHandler = services.GetRequiredService<IUserProfileHandler>();
    private readonly IPersonaRepository _repository = services.GetRequiredService<IPersonaRepository>();
    private readonly ITaskHandler _taskHandler = services.GetRequiredService<ITaskHandler>();
    private readonly IAgentAccessor _connectionAccessor = services.GetRequiredService<IAgentAccessor>();

    public PersonaEntity[] List() => _repository.GetAll();

    public PersonaEntity? GetByKey(uint key) => _repository.FindByKey(key);
    public PersonaEntity? GetByName(string name) => _repository.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public PersonaEntity Create(Action<PersonaEntity> setUp)
        => _repository.Create(setUp);

    public void Add(PersonaEntity persona) {
        if (_repository.FindByKey(persona.Key) != null)
            throw new InvalidOperationException($"A persona with the key '{persona.Key}' already exists.");

        _repository.Add(persona);
        logger.LogInformation("Added new persona: {PersonaKey} => {PersonaName}", persona.Name, persona.Key);
    }

    public void Update(PersonaEntity persona) {
        if (_repository.FindByKey(persona.Key) == null)
            throw new InvalidOperationException($"Persona with key '{persona.Key}' not found.");

        _repository.Update(persona);
        logger.LogInformation("Updated persona: {PersonaKey} => {PersonaName}", persona.Name, persona.Key);
    }

    public void Remove(uint key) {
        var persona = _repository.FindByKey(key)
                     ?? throw new InvalidOperationException($"Persona with key '{key}' not found.");

        _repository.Remove(key);
        logger.LogInformation("Removed persona: {PersonaKey} => {PersonaName}", persona.Name, persona.Key);
    }

    public async Task<Query[]> GenerateQuestions(PersonaEntity persona) {
        try {
            var appModel = _modelHandler.Internal ?? throw new InvalidOperationException("No default AI model selected.");
            var httpConnection = _connectionAccessor.GetFor(appModel.Provider!.Name);
            var userProfileEntity = _userHandler.Get() ?? throw new InvalidOperationException("No user found.");
            var personaEntity = GetByKey(1) ?? throw new InvalidOperationException("Required persona not found. Name: 'Agent Creator'.");
            var taskEntity = _taskHandler.GetByKey(1) ?? throw new InvalidOperationException("Required task not found. Name: 'Ask Questions about the AI Agent'.");
            var context = new JobContext {
                Model = appModel,
                Agent = httpConnection,
                UserProfile = userProfileEntity,
                Persona = personaEntity,
                Task = taskEntity,
            };
            var job = new Job(context);
            context.Input = persona;
            var result = await job.Execute(CancellationToken.None);
            if (result.HasException) throw new("Failed to generate next question: " + result.Exception.Message);
            var response = ((List<object>)context.Output).OfType<Dictionary<string, object>>().ToArray(i => new Map(i));
            return response.Select(i => new Query {
                Question = i.GetValueAs<string>(nameof(Query.Question)),
                Explanation = i.GetValueAs<string>(nameof(Query.Explanation)),
            }).ToArray();
        }
        catch (Exception ex) {
            logger.LogError(ex, "Error generating next question for persona {PersonaName}", persona.Name);
            throw;
        }
    }

    public async Task GeneratePersonaProperties(PersonaEntity persona) {
        try {
            var appModel = _modelHandler.Internal ?? throw new InvalidOperationException("No default AI model selected.");
            var httpConnection = _connectionAccessor.GetFor(appModel.Provider!.Name);
            var userProfileEntity = _userHandler.Get() ?? throw new InvalidOperationException("No user found.");
            var personaEntity = GetByKey(1) ?? throw new InvalidOperationException("Required persona not found. Name: 'Agent Creator'.");
            var taskEntity = _taskHandler.GetByKey(2) ?? throw new InvalidOperationException("Required task not found. Name: 'Ask Questions about the AI Agent'.");
            var context = new JobContext {
                Model = appModel,
                Agent = httpConnection,
                UserProfile = userProfileEntity,
                Persona = personaEntity,
                Task = taskEntity,
                Input = persona,
            };
            var job = new Job(context);
            job.Converters.Add(typeof(List<Query>),
                               v => {
                                   var list = (List<Query>)v;
                                   if (list.Count == 0) return string.Empty;
                                   var sb = new StringBuilder();
                                   foreach (var item in list) {
                                       sb.AppendLine($"Q: {item.Question}");
                                       sb.AppendLine($"{item.Explanation}");
                                       sb.AppendLine($"A: {item.Answer}");
                                   }
                                   return sb.ToString();
                               });
            var result = await job.Execute(CancellationToken.None);
            if (result.HasException) throw new("Failed to generate next question: " + result.Exception.Message);
            var response = new Map((Dictionary<string, object>)context.Output);
            persona.Expertise = response.GetValueAs<string>(nameof(Persona.Expertise));
            persona.Traits.AddRange(((List<object>)response[nameof(Persona.Traits)]).OfType<string>());
            persona.Important.AddRange(((List<object>)response[nameof(Persona.Important)]).OfType<string>());
            persona.Negative.AddRange(((List<object>)response[nameof(Persona.Negative)]).OfType<string>());
            persona.Other.AddRange(((List<object>)response[nameof(Persona.Other)]).OfType<string>());
        }
        catch (Exception ex) {
            logger.LogError(ex, "Error generating next question for persona {PersonaName}", persona.Name);
            throw;
        }
    }

    //public async Task<string> GeneratePrompt(PersonaEntity persona) {
    //    try {
    //        var appAgent = _agentHandler.Selected ?? throw new InvalidOperationException("No AI agent selected.");
    //        var aiAgent = _aiAgentFactory.Create(appAgent.Provider);

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

    //        var result = await job.ExecuteAsync(chat, CancellationToken.None);
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
