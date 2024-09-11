using DotNetToolbox.AI.Personas;

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

    public async Task<Query[]> GeneratePersonaCreationQuestion(PersonaEntity persona) {
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
            var response = context.OutputAsMap.GetList<Map>("Questions");
            return response.ToArray(i => new Query {
                Question = i.GetRequiredValueAs<string>(nameof(Query.Question)),
                Explanation = i.GetRequiredValueAs<string>(nameof(Query.Explanation)),
            });
        }
        catch (Exception ex) {
            logger.LogError(ex, "Error generating next question for persona {PersonaName}", persona.Name);
            throw;
        }
    }

    public async Task UpdateCreatedPersona(PersonaEntity persona) {
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
            persona.Role = context.OutputAsMap.GetRequiredValueAs<string>(nameof(Persona.Role));
            persona.Goals = context.OutputAsMap.GetRequiredList<string>(nameof(Persona.Goals));
            persona.Expertise = context.OutputAsMap.GetRequiredValueAs<string>(nameof(Persona.Expertise));
            persona.Traits = context.OutputAsMap.GetRequiredList<string>(nameof(Persona.Traits));
            persona.Important = context.OutputAsMap.GetRequiredList<string>(nameof(Persona.Important));
            persona.Negative = context.OutputAsMap.GetRequiredList<string>(nameof(Persona.Negative));
            persona.Other = context.OutputAsMap.GetRequiredList<string>(nameof(Persona.Other));
        }
        catch (Exception ex) {
            logger.LogError(ex, "Error generating next question for persona {PersonaName}", persona.Name);
            throw;
        }
    }
}
