namespace DotNetToolbox.ConsoleApplication.Application;

public interface IApplicationCreator<out TApplication, out TBuilder>
    where TApplication : class, IApplication<TApplication, TBuilder>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder> {
    public static abstract TApplication Create(string[] args, Action<IConfigurationBuilder> setConfiguration, Action<TBuilder> configureBuilder);
    public static abstract TApplication Create(Action<IConfigurationBuilder> setConfiguration, Action<TBuilder> configureBuilder);
    public static abstract TApplication Create(string[] args, Action<TBuilder> configureBuilder);
    public static abstract TApplication Create(Action<TBuilder> configureBuilder);
    public static abstract TApplication Create(string[] args, Action<IConfigurationBuilder> setConfiguration);
    public static abstract TApplication Create(Action<IConfigurationBuilder> setConfiguration);
    public static abstract TApplication Create(string[] args);
    public static abstract TApplication Create();
}
