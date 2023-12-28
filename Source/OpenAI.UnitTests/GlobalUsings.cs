global using System.Net;
global using System.Runtime.CompilerServices;
global using System.Text.Json;

global using DotNetToolbox.Core.TestUtilities.Extensions;
global using DotNetToolbox.Http;
global using DotNetToolbox.Http.Options;
global using DotNetToolbox.OpenAI.Chats;
global using DotNetToolbox.OpenAI.Chats.DataModels;
global using DotNetToolbox.OpenAI.HttpProvider;
global using DotNetToolbox.OpenAI.Models;
global using DotNetToolbox.OpenAI.Models.DataModels;
global using DotNetToolbox.OpenAI.Utilities;

global using FluentAssertions;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Logging;

global using NSubstitute;

global using Xunit;

global using static System.Text.Json.JsonSerializer;

global using OpenAIModel = DotNetToolbox.OpenAI.Models.DataModels.Model;
