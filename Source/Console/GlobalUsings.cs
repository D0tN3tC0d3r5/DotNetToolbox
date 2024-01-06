global using System.Reflection;
global using System.Text;
global using System.Text.RegularExpressions;

global using ConsoleApplication.Builders;
global using ConsoleApplication.Exceptions;
global using ConsoleApplication.Nodes;
global using ConsoleApplication.Nodes.Application;
global using ConsoleApplication.Nodes.Arguments;
global using ConsoleApplication.Nodes.Executables;
global using ConsoleApplication.Utilities;

global using DotNetToolbox;
global using DotNetToolbox.DependencyInjection;
global using DotNetToolbox.Results;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Logging.Abstractions;
global using Microsoft.Extensions.Options;

global using static System.Text.RegularExpressions.RegexOptions;

global using static DotNetToolbox.Ensure;
global using static DotNetToolbox.Results.Result;
