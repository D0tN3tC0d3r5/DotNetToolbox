### Console (DotNetToolbox.Console)

### Introduction
DotNetToolbox.Console is a robust and versatile framework for building command-line interface (CLI) and shell applications in .NET.

### Table of Contents
1. [Installation](#installation)
1. [Dependencies](#dependencies)
1. [Key Features](#key-features)
1. [Examples](#examples)


### Installation
```shell
PM> Install-Package DotNetToolbox.Core
```
### Dependencies
- .NET 8

#### Key Features
- **Command-Line Application Creation**: Create feature-rich CLI applications with ease.
- **Shell Application Creation**: Create interactive shell applications.
- **Extensive Customization**: Use commands, flags, options, and parameters to tailor your application. Also leverage a variety of options for application configuration and setup.

#### Examples
**The simplest CLI Console application ever**:
`SampleCLI.csproj`:
```xml
<Project  Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
 
  <ItemGroup>
    <PackageReference  Include="DotNetToolbox.Console"  Version="8.0.4" />
  </ItemGroup>
</Project>
```

`Program.cs`: 
```csharp
CommandLineInterfaceApplication.Create(args).Run(); // Yep, that's all.
```

Console output:
```shell
[user@host]> samplecli
SampleCLI v1.0.0.0

Usage:
    SampleCLI [Options]

Options:
    --clear-screen, -cls      Clear the screen.
    --help, -h, -?            Display this help information.
    --version                 Display the application's version.

[user@host]> samplecli
SampleCLI v1.0.0.0
[user@host]> samplecli --oops
Validation error: Unknown argument '--oops'. For a list of available arguments use '--help'.

>
```
**The simplest Shell application ever (async version)**:
`SampleShell.csproj`:
```xml
<Project  Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
 
  <ItemGroup>
    <PackageReference  Include="DotNetToolbox.Console"  Version="8.0.4" />
  </ItemGroup>
</Project>
```

`Program.cs`: 
```csharp
await ShellApplication.Create(args).RunAsync(); // Yep, that's all.
```
Output:
```shell
[user@host]> sampleshell
SampleShell v1.0.0.0
> help
SampleShell v1.0.0.0
This a sample console shell application.

Usage:
    SampleShell [Options] [Commands]

Options:
    --clear-screen, -cls      Clear the screen.
    --help, -h, -?            Display this help information.
    --version                 Display the application's version.

Commands:
    ClearScreen, cls          Clear the screen.
    Exit                      Exit the application.
    Help, ?                   Display this help information.

> ?
SampleShell v1.0.0.0
This a sample console shell application.

Usage:
    SampleShell [Options] [Commands]

Options:
    --clear-screen, -cls      Clear the screen.
    --help, -h, -?            Display this help information.
    --version                 Display the application's version.

Commands:
    ClearScreen, cls          Clear the screen.
    Exit                      Exit the application.
    Help, ?                   Display this help information.

> exit
[user@host]> sampleshell --help
SampleShell v1.0.0.0
This a sample console shell application.

Usage:
    SampleShell [Options] [Commands]

Options:
    --clear-screen, -cls      Clear the screen.
    --help, -h, -?            Display this help information.

Commands:
    ClearScreen, cls          Clear the screen.
    Exit                      Exit the application.
    Help, ?                   Display this help information.

SampleShell v1.0.0.0
> exit
[user@host]> 
```

#### Adding Arguments (flags, options,  and parameters)
- **Flag**: a simple boolean argument to toggle features in your application. Ex: yourapp --verbose, yourapp -v;  (sets the verbose mode on.) 
-  The **flag** can also execute an action but the action should not expect any arguments: Ex: yourapp --help (displays the help and exits) 
- `AddFlag(string name, Delegate action)`
- `AddFlag(string name, string alias, Delegate action)`
- `AddFlag(string name, string[] aliases, Delegate action)`
- `AddFlag<TFlag>()`: Use this to add the class that implement the flag behavior.
- **Option**: an argument that expets a value takes a value to customize the behavior of your application. Ex. yourapp --delay 3000, yourapp -d 500  (sets the value of the delay option)
- `AddOption(string name)`
- `AddOption(string name, string alias)`
- `AddOption(string name, string[] aliases)`
- `AddOption<TOption>()`: Use this to add the class that implement the option behavior.

- **Parameter**: a value to be assigned to a positional argument. If it has a default value it is considered optional. Ex: yourapp 42  (sets a pre-defined parameter with the value 42)
- `AddParameter(string name)`
- `AddParameter(string name, string? default)`
- `AddParameter<TParameter>()`: Use this to add the class that implement the parameters behavior.

#### Adding Commands
- **Command**: a command is a argument that executes an action. The command itself can have arguments. Ex: yourapp say --message "Hello world!"
- `AddCommand(string name, Delegate action)`
- `AddCommand(string name, string alias, Delegate action)`
- `AddCommand(string name, string[] aliases, Delegate action)`
- `AddCommand<TCommand>()`: Use this to add the class that implement the command behavior.

#### Configuring your application
The `Create` method of the application accepts a delegate to configure it through the application builder.

**Application Builder Options**:

- `SetEnvironment(string environment)` - This method allows you to set the environment of the application. If not set the environment is set to empty which is equivalent to "Production".

The following methods allow you to load configuration from many sources. All the values are loaded into the application context as a key/value pair.
- `AddEnvironmentVariables(string? prefix = null)` => This method loads the values of the system environment variables. The optional prefix parameter will load only the variables that starts with the provided prefix. 
- `AddUserSecrets<TReference>()` => This method loads the values from the assembly where the `TReference` type is defined. 
- `AddAppSettings(IFileProvider? fileProvider = null)` => This method loads the values from the assembly where the `TReference` type is defined. 
- `AddValue(string key, object value)` => This method sets one key/value pair in the context. 
- `ConfigureOptions(Action<TOptions> configure)` => This method sets a predefined strongly type options classe and loads it into the context. 

The following method allows you to configure the logging used in the application.
- `ConfigureLogging(Action<ILoggingBuilder> configure)` - 

The following methods enable you to replace some of the framework static functionality. One of the main uses for those methods, is to enable the application to be fully unit testable.
- `SetAssemblyInformation(IAssemblyDescriptor assemblyDescriptor)` => Enables to replace the values of the application assembly.
- `SetInputHandler(IInput input)` => Enables to replace the console functionality that captures the user input.
- `SetOutputHandler(IOutput output)` => Enables to replace the console functionality that write values to the screen. 
- `SetFileSystem(IFileSystem fileSystem)` => Enables to replace the basic file and directory manipulation done by the OS. 
- `SetGuidProvider(IGuidProvider guidProvider)` => Enables to replace the generation of GUIDs by the system. 
- `SetDateTimeProvider(IDateTimeProvider dateTimeProvider)` => Enables to replace the generation of Date and Time values by the system. 

#### Examples
Here is a much more complex code application and how It could be tested.
```csharp
using DotNetToolbox.ConsoleApplication.Nodes;
using DotNetToolbox.Results;

var app = BigMouth.Create(args, b => {
    b.AddAppSettings(); // This will add the values from appsettings.json to the context
    b.AddUserSecrets<Program>(); // This will add the values from the user secrets to the context
});

await app.RunAsync();

public class BigMouth
    : ShellApplication<BigMouth> {
    public BigMouth(string[] args, IServiceProvider services)
        : base(args, services) {
        AddCommand<SayCommand>();
    }

    protected override Task<Result> Execute(CancellationToken ct) {
        var name = Context["MyName"];
        Environment.Output.WriteLine($"Hello {name}.");
        return base.Execute(ct);
    }
}

public class SayCommand : Command<SayCommand> {
    public SayCommand(IHasChildren parent)
        : base(parent, "Say", ["s"]) {
        AddOption("My");
        AddParameter("Info", "Color");
    }

    public override Task<Result> Execute(CancellationToken ct = default) {
        var name = Application.Context["MyName"];
        Context.TryGetValue("My", out var type);
        type = type?.Equals("secret", StringComparison.CurrentCultureIgnoreCase) ?? false ? "Secret" : "Public";
        Context.TryGetValue("Info", out var info);
        Application.Context.TryGetValue($"{type}{info}", out var secret);
        Environment.Output.WriteLine(secret != null
                                         ? $"Ok {name}. Your {type} {info} is {secret}."
                                         : "I don't know.");
        return base.Execute(ct);
    }
}
```
Output:
```shell
[user@host]> .\bigmouth
BigMouth v1.0.0.0
Hello Dave.
> say
Ok Dave. Your Public Color is Green.
> say --my secret color
I don't know.
> say --my secret message
I don't know.
> exit
```

