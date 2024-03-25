namespace DotNetToolbox.AI.Agents;

public interface IAgentFactory {
    TAgent Create<TAgent>(string provider)
        where TAgent : class, IAgent;
    //TAgent Create<TAgent>(string provider, World world, Persona persona, IAgentOptions options)
    //    where TAgent : class, IAgent;
}

//public interface IProviderAgentFactory {
//    TAgent Create<TAgent>(World world, Persona persona, IAgentOptions options)
//        where TAgent : class, IAgent;
//}
