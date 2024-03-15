namespace DotNetToolbox.ConsoleApplication.Application;

public interface IBuilderCreator<TApplication, out TBuilder>
    where TApplication : class, IApplication<TApplication, TBuilder>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder> {
    public static abstract TBuilder CreateBuilder(Action<IConfigurationBuilder>? setConfiguration = null);
    public static abstract TBuilder CreateBuilder(string[] args, Action<IConfigurationBuilder>? setConfiguration = null);
}
