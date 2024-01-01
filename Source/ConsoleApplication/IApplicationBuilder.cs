namespace DotNetToolbox.ConsoleApplication;

public interface IApplicationBuilder<out TApplication, TBuilder, TOptions>
    where TApplication : class, IApplication<TApplication, TBuilder, TOptions>
    where TBuilder : class, IApplicationBuilder<TApplication, TBuilder, TOptions>
    where TOptions : class, IApplicationOptions, new() {
    TApplication Build();
}
