global using System.Net;

global using DotNetToolbox.AI.Consumers;
global using DotNetToolbox.AI.OpenAI;
global using DotNetToolbox.AI.OpenAI.Chat;
global using DotNetToolbox.AI.OpenAI.Model;
global using DotNetToolbox.AI.Personas;
global using DotNetToolbox.AI.Utilities;
global using DotNetToolbox.Http;
global using DotNetToolbox.TestUtilities.Logging;

global using FluentAssertions;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;

global using NSubstitute;

global using Xunit;

global using static System.Text.Json.JsonSerializer;

global using static DotNetToolbox.AI.OpenAI.AgentOptions;