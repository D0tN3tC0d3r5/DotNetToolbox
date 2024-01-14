global using System.Diagnostics.CodeAnalysis;
global using System.Reflection;
global using System.Text;
global using System.Text.Json;
global using System.Text.RegularExpressions;

global using DotNetToolbox;
global using DotNetToolbox.ConsoleApplication.Builders;
global using DotNetToolbox.ConsoleApplication.Exceptions;
global using DotNetToolbox.ConsoleApplication.Nodes;
global using DotNetToolbox.ConsoleApplication.Nodes.Application;
global using DotNetToolbox.ConsoleApplication.Nodes.Arguments;
global using DotNetToolbox.ConsoleApplication.Nodes.Executables;
global using DotNetToolbox.ConsoleApplication.Utilities;
global using DotNetToolbox.DependencyInjection;
global using DotNetToolbox.Results;
global using DotNetToolbox.Singleton;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Configuration.Json;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.FileProviders;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Logging.Abstractions;
global using Microsoft.Extensions.Options;

global using static System.Text.RegularExpressions.RegexOptions;

global using static DotNetToolbox.Ensure;
global using static DotNetToolbox.Results.Result;
