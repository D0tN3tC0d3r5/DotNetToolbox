namespace AI.Sample.Agents.Repositories;

public class AgentRepository(IAgentRepositoryStrategy strategy)
    : Repository<IAgentRepositoryStrategy, AgentEntity, uint>(strategy),
      IAgentRepository;
