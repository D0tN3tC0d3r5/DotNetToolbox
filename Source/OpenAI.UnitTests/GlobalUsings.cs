global using System.Net;

global using DotNetToolbox.Http;
global using DotNetToolbox.OpenAI.DataModels;
global using DotNetToolbox.OpenAI.Utilities;
global using DotNetToolbox.Results;
global using DotNetToolbox.TestUtilities.Logging;

global using FluentAssertions;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;

global using NSubstitute;
global using NSubstitute.ExceptionExtensions;

global using Xunit;

global using static System.Text.Json.JsonSerializer;

global using static DotNetToolbox.OpenAI.Agents.ChatOptions;

global using OpenAIModel = DotNetToolbox.OpenAI.DataModels.Model;
