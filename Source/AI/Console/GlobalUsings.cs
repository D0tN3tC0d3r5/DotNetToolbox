// Global using directives
global using System;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics.CodeAnalysis;
global using System.IO;
global using System.Linq.Expressions;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Security.Cryptography;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;

global using AI.Sample;
global using AI.Sample.Agents.Handlers;
global using AI.Sample.Helpers;
global using AI.Sample.Main.Commands;
global using AI.Sample.Models.Commands;
global using AI.Sample.Models.Handlers;
global using AI.Sample.Models.Repositories;
global using AI.Sample.Personas.Commands;
global using AI.Sample.Personas.Handlers;
global using AI.Sample.Personas.Repositories;
global using AI.Sample.Providers.Commands;
global using AI.Sample.Providers.Handlers;
global using AI.Sample.Providers.Repositories;
global using AI.Sample.Tasks.Commands;
global using AI.Sample.Tasks.Handlers;
global using AI.Sample.Tasks.Repositories;
global using AI.Sample.UserProfile.Commands;
global using AI.Sample.UserProfile.Handlers;
global using AI.Sample.UserProfile.Repositories;

global using DotNetToolbox;
global using DotNetToolbox.AI.Agents;
global using DotNetToolbox.AI.Anthropic;
global using DotNetToolbox.AI.Jobs;
global using DotNetToolbox.AI.OpenAI;
global using DotNetToolbox.AI.Personas;
global using DotNetToolbox.ConsoleApplication;
global using DotNetToolbox.ConsoleApplication.Application;
global using DotNetToolbox.ConsoleApplication.Commands;
global using DotNetToolbox.ConsoleApplication.Nodes;
global using DotNetToolbox.Data.File;
global using DotNetToolbox.Data.Repositories;
global using DotNetToolbox.Data.Strategies;
global using DotNetToolbox.Domain.Models;
global using DotNetToolbox.Environment;
global using DotNetToolbox.Results;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Options;

global using Serilog;

global using Spectre.Console;

global using static System.Ensure;
